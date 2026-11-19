using System.Reflection;
using System.Text.Json;

namespace Aban360.Tests.ServiceBaselines;

// Explicit mapping between the new application boundary and frozen handler contracts.
// This lets older controller comparisons keep checking calls, inputs and ordering.
public static class ApplicationServiceBaseline
{
    public static readonly ServiceOperation[] Operations = new[] { "ServiceConsolidation15.json", "ServiceConsolidation30.json", "ServiceConsolidation20.json", "ServiceConsolidation19.json", "ServiceConsolidationWave20.json" }
        .SelectMany(file => JsonSerializer.Deserialize<ServiceOperation[]>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, file)))!).ToArray();
    public static readonly ServiceOperation[] CriticalOperations = JsonSerializer.Deserialize<ServiceOperation[]>(
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "ServiceConsolidationCritical10.json")))!;
    public static readonly ServiceOperation[] AuxiliaryOperations = JsonSerializer.Deserialize<ServiceOperation[]>(
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "ServiceConsolidationAuxiliary10.json")))!;
    public static readonly ServiceOperation[] ManagementOperations = JsonSerializer.Deserialize<ServiceOperation[]>(
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "ServiceConsolidationManagement20.json")))!;
    public static readonly ServiceOperation[] WorkflowOperations = JsonSerializer.Deserialize<ServiceOperation[]>(
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "ServiceConsolidationWorkflow10.json")))!;
    public static readonly ServiceOperation[] Operations19Operations = JsonSerializer.Deserialize<ServiceOperation[]>(
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "ServiceConsolidationOperations19.json")))!;
    private static readonly Dictionary<(string, string), string> Names = Operations.Concat(CriticalOperations).Concat(AuxiliaryOperations).Concat(ManagementOperations).Concat(WorkflowOperations).ToDictionary(
        o => (o.Service, o.Method), o => o.OldContract + "." + o.OldMethod);
    private static readonly Dictionary<(string,string),string> Operations19Names = new()
    {
        [("Aban360.CalculationPool.Application.Features.Sale.Services.IArticle11Service", "Article11GetAllAsync") ] = "Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts.IArticle11GetAllHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.Sale.Services.IArticle11Service", "Article11GetAsync") ] = "Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts.IArticle11GetHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.Sale.Services.IInstallationAndEquipmentService", "InstallationAndEquipmentGetAllAsync") ] = "Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts.IInstallationAndEquipmentGetAllHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.Sale.Services.IInstallationAndEquipmentService", "InstallationAndEquipmentGetAsync") ] = "Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts.IInstallationAndEquipmentGetHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.Sale.Services.ITankerWaterDistanceTariffService", "TankerWaterDistanceTariffGetAllAsync") ] = "Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts.ITankerWaterDistanceTariffGetAllHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.Sale.Services.ITankerWaterDistanceTariffService", "TankerWaterDistanceTariffGetAsync") ] = "Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts.ITankerWaterDistanceTariffGetHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IOtherExpensesItemsService", "OtherExpensesItemsInsertAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts.IOtherExpensesItemsInsertHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IOtherExpensesItemsService", "OtherExpensesItemsRemoveAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations.IOtherExpensesItemsRemoveHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IOtherExpensesItemsService", "OtherExpensesItemsGetAllAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts.IOtherExpensesItemsGetAllHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IOtherExpensesItemsService", "OtherExpensesItemsGetByIdAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts.IOtherExpensesItemsGetByIdHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IOtherExpensesItemsService", "OtherExpensesItemsGetByItemIdAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts.IOtherExpensesItemsGetByItemIdHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IServiceLinkPaymentService", "ServiceLinkDeleteAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts.IServiceLinkDeleteHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IServiceLinkPaymentService", "ServiceLinkRegisterManualAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts.IServiceLinkRegisterManualHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IServiceLinkPaymentService", "ServiceLinkPaidGetAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts.IServiceLinkPaidGetHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IServiceLinkReturnService", "ServiceLinkReturnDisconnectAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts.IServiceLinkReturnDisconnectHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IServiceLinkReturnService", "ServiceLinkReturnAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts.IServiceLinkReturnHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IServiceLinkReturnService", "ServiceLinkReturnReturnCodesAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts.IServiceLinkReturnHandler.ReturnCodes",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IServiceLinkReturnService", "ServiceLinkReturnRemoveAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts.IServiceLinkReturnRemoveHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IServiceLinkBatchReturnService", "ServiceLinkRemoveUnconfirmedAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts.IServiceLinkRemoveUnconfirmedHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IServiceLinkBatchReturnService", "ServiceLinkReturnConfirmeAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts.IServiceLinkReturnConfirmeHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IServiceLinkBatchReturnService", "ServiceLinkTempReturnAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts.IServiceLinkTempReturnHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.ServiceLink.Services.IServiceLinkBatchReturnService", "ServiceLinkUnconfirmedGetAsync") ] = "Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts.IServiceLinkUnconfirmedGetHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterReadingDetailService", "MeterReadingDetailExcludeAsync") ] = "Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts.IMeterReadingDetailExcludeHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterReadingDetailService", "MeterReadingDetailUpdateAsync") ] = "Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts.IMeterReadingDetailUpdateHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterReadingCartableService", "CartableByZoneIdGetAsync") ] = "Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts.ICartableByZoneIdGetHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterReadingCartableService", "CartableAsync") ] = "Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts.ICartableHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterReadingCheckedListService", "CheckedListGetAsync") ] = "Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts.ICheckedListGetHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterReadingCheckedListService", "CheckedListGetGetReadingControlAsync") ] = "Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts.ICheckedListGetHandler.GetReadingControl",
        [("Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterReadingCheckedListService", "MeterReadingDetailCompletedAsync") ] = "Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts.IMeterReadingDetailCompletedHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterFlowService", "CalculationConfirmationAsync") ] = "Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts.ICalculationConfirmationHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterFlowService", "MeterFlowValidationGetAsync") ] = "Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts.IMeterFlowValidationGetHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterFlowService", "ConsumptionCheckedAsync") ] = "Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts.IConsumptionCheckedHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterFlowService", "InitialCalculationAsync") ] = "Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts.IInitialCalculationHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterFlowService", "MeterReadingCalculationConfirmThenSendSmsAsync") ] = "Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts.IMeterReadingCalculationConfirmThenSendSmsHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.Sale.Services.ITankerWaterOperationsService", "TankerWaterSetPayAsync") ] = "Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts.ITankerWaterSetPayHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.Sale.Services.ITankerWaterOperationsService", "TankerWaterCalculationAsync") ] = "Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts.ITankerWaterCalculationHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.CollectBills.Services.ICollectBillsFilesService", "CollectBillsDetailFileDownloadAsync") ] = "Aban360.CalculationPool.Application.Features.CollectBills.Handlers.Commands.Contracts.ICollectBillsDetailFileDownloadHandler.Handle",
        [("Aban360.CalculationPool.Application.Features.CollectBills.Services.ICollectBillsFilesService", "CollectBillsFileCreateAsync") ] = "Aban360.CalculationPool.Application.Features.CollectBills.Handlers.Commands.Implementations.ICollectBillsFileCreateHandler.Handle",
        [("Aban360.OldCalcPool.Application.Features.Processing.Services.IBillGenerationService", "FreeGenerateBillAsync") ] = "Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts.IFreeGenerateBillHandler.Handle",
        [("Aban360.OldCalcPool.Application.Features.Processing.Services.IBillGenerationService", "GenerateBillAsync") ] = "Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts.IGenerateBillHandler.Handle",
        [("Aban360.OldCalcPool.Application.Features.Processing.Services.IBillGenerationService", "GenerateBillWithRemainedAsync") ] = "Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts.IGenerateBillWithRemainedHandler.Handle",
        [("Aban360.OldCalcPool.Application.Features.Processing.Services.IBillGenerationService", "NowGenerateBillAsync") ] = "Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts.INowGenerateBillHandler.Handle",
        [("Aban360.PaymentPool.Application.Features.BankReceipt.Services.IBankReceiptService", "BankReceiptDeleteAsync") ] = "Aban360.PaymentPool.Application.Features.BankReceipt.Contracts.IBankReceiptDeleteHandler.Handle",
        [("Aban360.PaymentPool.Application.Features.BankReceipt.Services.IBankReceiptService", "BankReceiptFlowUploadAsync") ] = "Aban360.PaymentPool.Application.Features.BankReceipt.Contracts.IBankReceiptFlowHandler.Upload",
        [("Aban360.PaymentPool.Application.Features.BankReceipt.Services.IBankReceiptService", "BankReceiptFlowListAsync") ] = "Aban360.PaymentPool.Application.Features.BankReceipt.Contracts.IBankReceiptFlowHandler.List",
        [("Aban360.PaymentPool.Application.Features.BankReceipt.Services.IBankReceiptService", "BankReceiptFlowValidateAsync") ] = "Aban360.PaymentPool.Application.Features.BankReceipt.Contracts.IBankReceiptFlowHandler.Validate",
        [("Aban360.PaymentPool.Application.Features.BankReceipt.Services.IBankReceiptService", "BankReceiptFlowDisplayAsync") ] = "Aban360.PaymentPool.Application.Features.BankReceipt.Contracts.IBankReceiptFlowHandler.Display",
        [("Aban360.PaymentPool.Application.Features.BankReceipt.Services.IBankReceiptService", "BankReceiptFlowReconcileAsync") ] = "Aban360.PaymentPool.Application.Features.BankReceipt.Contracts.IBankReceiptFlowHandler.Reconcile",
        [("Aban360.PaymentPool.Application.Features.BankReceipt.Services.IBankReceiptService", "BankReceiptFlowConfirmAsync") ] = "Aban360.PaymentPool.Application.Features.BankReceipt.Contracts.IBankReceiptFlowHandler.Confirm",
        [("Aban360.PaymentPool.Application.Features.BankReceipt.Services.IBankReceiptService", "BankReceiptDiscrepancyReportAsync") ] = "Aban360.PaymentPool.Application.Features.BankReceipt.Contracts.IBankReceiptDiscrepancyHandler.Report",
        [("Aban360.PaymentPool.Application.Features.BankReceipt.Services.IBankReceiptService", "BankReceiptDiscrepancyDescribeAsync") ] = "Aban360.PaymentPool.Application.Features.BankReceipt.Contracts.IBankReceiptDiscrepancyHandler.Describe",
        [("Aban360.PaymentPool.Application.Features.BankReceipt.Services.IBankReceiptService", "BankReceiptBankGetAllAsync") ] = "Aban360.PaymentPool.Application.Features.BankReceipt.Contracts.IBankReceiptBankGetAllHandler.Handle",
        [("Aban360.BlobPool.Application.Features.OpenKm.Services.IOpenKmFilesService", "AddFileAsync") ] = "Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts.IAddFileHandler.Handle",
        [("Aban360.BlobPool.Application.Features.OpenKm.Services.IOpenKmFilesService", "RemoveFileAsync") ] = "Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts.IRemoveFileHandler.Handle",
        [("Aban360.BlobPool.Application.Features.OpenKm.Services.IOpenKmFilesService", "GetDocumentBinaryAsync") ] = "Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts.IGetDocumentBinaryHandler.Handle",
        [("Aban360.BlobPool.Application.Features.OpenKm.Services.IOpenKmFilesService", "GetDownloadLinkAsync") ] = "Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts.IGetDownloadLinkHandler.Handle",
        [("Aban360.BlobPool.Application.Features.OpenKm.Services.IOpenKmFilesService", "DisplayRemovedFilesAsync") ] = "Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts.IDisplayRemovedFilesHandler.Handle",
        [("Aban360.BlobPool.Application.Features.OpenKm.Services.IOpenKmFoldersMetadataService", "CreateFolderAsync") ] = "Aban360.BlobPool.Application.Features.OpenKm.Handlers.Querys.Contracts.ICreateFolderHandler.Handle",
        [("Aban360.BlobPool.Application.Features.OpenKm.Services.IOpenKmFoldersMetadataService", "AddOrUpdateMetaDataAsync") ] = "Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts.IAddOrUpdateMetaDataHandler.Handle",
        [("Aban360.BlobPool.Application.Features.OpenKm.Services.IOpenKmFoldersMetadataService", "GetMetaDataPropertiesAsync") ] = "Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts.IGetMetaDataPropertiesHandler.Handle",
        [("Aban360.BlobPool.Application.Features.OpenKm.Services.IOpenKmFoldersMetadataService", "CopyTrackImagesToBillAsync") ] = "Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts.ICopyTrackImagesToBillHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestLifecycleService", "CloseRequestAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts.ICloseRequestHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestLifecycleService", "PreviousStatusRequestAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts.IPreviousStatusRequestHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestLifecycleService", "ReferredToIndividualRequestAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts.IReferredToIndividualRequestHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestLifecycleService", "MoshtrakRequestUpdateAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts.IMoshtrakRequestUpdateHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestLifecycleService", "SwapRequestTypeAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts.ISwapRequestTypeHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestLifecycleService", "ArchiveRequestAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts.IArchiveRequestHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestLifecycleService", "ArchivedRequestAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts.IArchivedRequestHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestDisplayService", "DisplayRequestAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts.IDisplayRequestHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestDisplayService", "DisplayRequestByTrackIdAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations.IDisplayRequestByTrackIdHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestDisplayService", "KartableRequestGetAllAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts.IKartableRequestGetAllHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestDisplayService", "RequestBasicInfoGetAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts.IRequestBasicInfoGetHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestDisplayService", "PreviousRequestGetByBillIdAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts.IPreviousRequestGetByBillIdHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestDisplayService", "KitCodeDictionaryGetAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts.IKitCodeDictionaryGetHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestDisplayService", "ReadingBlockListGetAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts.IReadingBlockListGetHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestDisplayService", "ToCalulationConfirmAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations.IToCalulationConfirmHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestDisplayService", "ToSetAssessmentTimeGetByTrackIdAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations.IToSetAssessmentTimeGetByTrackIdHandler.Handle",
        [("Aban360.ClaimPool.Application.Features.Request.Services.IRequestDisplayService", "ToSetReAssessmentTimeGetByTrackIdAsync") ] = "Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations.IToSetReAssessmentTimeGetByTrackIdHandler.Handle",
    };
    public static string CallName(Type contract, MethodInfo method) =>
        Operations19Names.TryGetValue((contract.FullName!, method.Name), out var operation19) ? operation19 :
        contract.FullName == "Aban360.OldCalcPool.Application.Features.Db70.Services.IBillReturnCauseService" && method.Name == "GetAllDictionaryAsync"
            ? "Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts.IBillReturnCauseGetAllHandler.HandleByDictionary" :
        Names.TryGetValue((contract.FullName!, method.Name), out var name) ? name : contract.FullName + "." + method.Name;
}

public record ServiceOperation(string Feature, string Assembly, string Original, string OldContract,
    string Service, string Method, string OldMethod);
