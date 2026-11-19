from pathlib import Path
import json,collections
D=json.loads(Path('tmp/next20-inventory.json').read_text(encoding='utf-8')); allowed=D['Allowed']; groups=[
('Article11','CalculationPool','Article11GetAll Article11Get'),
('InstallationAndEquipment','CalculationPool','InstallationAndEquipmentGetAll InstallationAndEquipmentGet'),
('TankerWaterDistanceTariff','CalculationPool','TankerWaterDistanceTariffGetAll TankerWaterDistanceTariffGet'),
('OtherExpensesItems','CalculationPool','OtherExpensesItemsInsert OtherExpensesItemsRemove OtherExpensesItemsGetAll OtherExpensesItemsGetById OtherExpensesItemsGetByItemId'),
('Tariff','CalculationPool','TariffCreate TariffDuplicate TariffDelete TariffUpdate TariffGetAll TariffGetByLineItemTypeId TariffGetByOfferingId TariffGetSingle'),
('ServiceLinkPayment','CalculationPool','ServiceLinkDelete ServiceLinkRegisterManual ServiceLinkPaidGet'),
('ServiceLinkReturn','CalculationPool','ServiceLinkReturnDisconnect ServiceLinkReturn ServiceLinkReturnRemove'),
('ServiceLinkBatchReturn','CalculationPool','ServiceLinkRemoveUnconfirmed ServiceLinkReturnConfirme ServiceLinkTempReturn ServiceLinkUnconfirmedGet'),
('MeterReadingDetail','CalculationPool','MeterReadingDetailExclude MeterReadingDetailUpdate'),
('MeterReadingCartable','CalculationPool','CartableByZoneIdGet Cartable'),
('MeterReadingCheckedList','CalculationPool','CheckedListGet MeterReadingDetailCompleted'),
('MeterFlow','CalculationPool','CalculationConfirmation MeterFlowValidationGet ConsumptionChecked InitialCalculation MeterReadingCalculationConfirmThenSendSms'),
('TankerWaterOperations','CalculationPool','TankerWaterSetPay TankerWaterCalculation'),
('CollectBillsFiles','CalculationPool','CollectBillsDetailFileDownload CollectBillsFileCreate'),
('BillGeneration','OldCalcPool','FreeGenerateBill GenerateBill GenerateBillWithRemained NowGenerateBill'),
('BankReceipt','PaymentPool','BankReceiptDelete BankReceiptFlow BankReceiptDiscrepancy BankReceiptBankGetAll'),
('OpenKmFiles','BlobPool','AddFile RemoveFile GetDocumentBinary GetDownloadLink DisplayRemovedFiles'),
('OpenKmFoldersMetadata','BlobPool','CreateFolder AddOrUpdateMetaData GetMetaDataProperties CopyTrackImagesToBill'),
('RequestLifecycle','ClaimPool','CloseRequest PreviousStatusRequest ReferredToIndividualRequest MoshtrakRequestUpdate SwapRequestType ArchiveRequest ArchivedRequest'),
('RequestDisplay','ClaimPool','DisplayRequest DisplayRequestByTrackId KartableRequestGetAll RequestBasicInfoGet PreviousRequestGetByBillId KitCodeDictionaryGet ReadingBlockListGet ToCalulationConfirm ToSetAssessmentTimeGetByTrackId ToSetReAssessmentTimeGetByTrackId')]
seen=set();selection=[]
for i,(feature,module,names) in enumerate(groups,51):
 for name in names.split():
  h,=[h for h in allowed if h['Name']==name+'Handler' and h['Module']==module]
  assert h['Source'] not in seen,h['Source'];seen.add(h['Source']);selection.append(dict(Feature=feature,Module=module,Name=h['Name'],Source=h['Source'],Consumers=h['Consumers']))
 print(i,feature,module,len(names.split()))
assert len(groups)==20
print('TOTAL SELECTED',len(seen),'REMAINING DIRECT AFTER PROPOSED LIST',len(allowed)-len(seen))
Path('tmp/next20-selection.json').write_text(json.dumps(selection,indent=2),encoding='utf-8')
