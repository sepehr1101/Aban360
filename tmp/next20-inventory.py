from pathlib import Path
import re,collections,json
root=Path('.')
handlers=[]
for d in root.glob('Aban360.*.Application'):
 for p in d.rglob('*Handler.cs'):
  if {'bin','obj'}&set(p.parts):continue
  s=p.read_text(encoding='utf-8-sig');name=p.stem
  m=re.search(r'\b((?:(?:public|internal|private|protected|sealed|abstract|partial|static)\s+)*)class\s+'+re.escape(name)+r'\b',s)
  if not m or 'abstract' in m[1].split():continue
  ns=re.search(r'\bnamespace\s+([\w.]+)',s)[1]
  handlers.append(dict(Name=name,Source=p.as_posix(),Namespace=ns,Module=d.name.split('.')[1],Contract='I'+name))
print('CONCRETE APPLICATION HANDLERS',len(handlers))
print('MODULES',dict(collections.Counter(h['Module'] for h in handlers)))
controllers=[]
for d in ['Aban360.Api','Aban360.BrdigeApi']:
 for p in Path(d).rglob('*Controller.cs'):
  if {'bin','obj'}&set(p.parts):continue
  s=p.read_text(encoding='utf-8-sig');routes=re.findall(r'\[(?:Route|HttpGet|HttpPost|HttpPatch|HttpPut|HttpDelete)\s*\(\s*"([^"]+)"',s)
  for h in handlers:
   if re.search(r'\b'+h['Contract']+r'\b',s):
    controller=dict(Path=p.as_posix(),Blocked=any('excel' in r.lower() or 'sti' in r.lower() for r in routes),Routes=routes)
    h.setdefault('Consumers',[]).append(controller)
blocked=[];allowed=[];unreferenced=[]
for h in handlers:
 if h['Module']=='ReportPool' or any(c['Blocked'] for c in h.get('Consumers',[])):blocked.append(h)
 elif h.get('Consumers'):allowed.append(h)
 else:unreferenced.append(h)
print('EXCLUDED report module / report-route consumers',len(blocked))
print('DIRECT non-report candidates',len(allowed),'NO DIRECT CONTROLLER (needs review)',len(unreferenced))
byctrl=collections.defaultdict(list)
for h in allowed:
 for c in h['Consumers']:byctrl[c['Path']].append(h['Name'].removesuffix('Handler'))
for c,names in sorted(byctrl.items()):print(c,':',', '.join(names))
Path('tmp/next20-inventory.json').write_text(json.dumps(dict(Handlers=handlers,Allowed=allowed,Excluded=blocked,Indirect=unreferenced),indent=2),encoding='utf-8')
