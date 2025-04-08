namespace Aban360.BlobPool.Persistence.Migrations.Enums
{
    internal enum TableName
    {
        DocumentCategory,// folder to group
        DocumentType, //arzyabi, qeraat, ...
        Document, //file stream,content,..., size
        ImageWatermark, 
        MimetypeCategory,// video, audio, ...
        ExecutableMimetype,//any file extention/mimetype that we can execute        
    }
}