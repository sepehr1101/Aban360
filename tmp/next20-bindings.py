from pathlib import Path
import json,re,collections
root=Path('.');data=json.loads(Path('tmp/next20-inventory.json').read_text(encoding='utf-8'));hs=data['Handlers'];contracts=collections.defaultdict(list)
for directory in root.glob('Aban360.*.Application'):
 for p in directory.rglob('*Handler.cs'):
  if {'bin','obj'}&set(p.parts):continue
  s=p.read_text(encoding='utf-8-sig')
  for name in re.findall(r'\binterface\s+(I\w+Handler)\b',s):
   ns=re.search(r'\bnamespace\s+([\w.]+)',s)[1];contracts[(directory.name,name)].append(ns)
controllers=[]
for directory in ['Aban360.Api','Aban360.BrdigeApi']:
 for p in Path(directory).rglob('*Controller.cs'):
  if {'bin','obj'}&set(p.parts):continue
  s=p.read_text(encoding='utf-8-sig');usings=re.findall(r'using\s+(?:(\w+)\s*=\s*)?((?:global::)?[\w.]+);',s);routes=re.findall(r'\[(?:Route|HttpGet|HttpPost|HttpPatch|HttpPut|HttpDelete)\s*\(\s*"([^"]+)"',s)
  controllers.append((p,s,usings,routes))
for h in hs:
 h['ContractNamespaces']=contracts[(h['Source'].split('/')[0],h['Contract'])];h['Consumers']=[]
 for p,s,usings,routes in controllers:
  if not re.search(r'\b'+h['Contract']+r'\b',s):continue
  matched=False
  for alias,namespace in usings:
   namespace=namespace.removeprefix('global::')
   if namespace in h['ContractNamespaces']:
    if not alias or re.search(r'\b'+alias+r'\.'+h['Contract']+r'\b',s):matched=True
  if any(ns+'.'+h['Contract'] in s for ns in h['ContractNamespaces']):matched=True
  if matched:h['Consumers'].append(dict(Path=p.as_posix(),Blocked=any('excel' in r.lower() or 'sti' in r.lower() for r in routes),Routes=routes))
allowed=[h for h in hs if h['Module']!='ReportPool' and h['Consumers'] and not any(c['Blocked'] for c in h['Consumers']) and h['Name']!='TrackingFlowGetHandler']
blocked=[h for h in hs if h['Module']=='ReportPool' or any(c['Blocked'] for c in h['Consumers'])]
indirect=[h for h in hs if h['Module']!='ReportPool' and not h['Consumers']]
print('PRECISE COUNT',len(hs),'direct candidates',len(allowed),'excluded reports/routes',len(blocked),'no direct controller',len(indirect),'reserved TrackingFlow',1)
print('INDIRECT:',', '.join(h['Name'] for h in indirect))
Path('tmp/next20-inventory.json').write_text(json.dumps(dict(Handlers=hs,Allowed=allowed,Excluded=blocked,Indirect=indirect),indent=2),encoding='utf-8')
