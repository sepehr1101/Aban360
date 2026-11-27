namespace Aban360.PaymentPool.Domain.Constansts
{
    public enum PaymentMethodEnum:short
    {
        ATM=2,
        PresenceAtBank= 3,
        Interanet=5,
        SMS=6,
        BankPhone=7,
        CRS=8,//Self Receiver
        PaymentBank=9,
        WebKiosk=13,
        POS=14,
        Internet=59,
    }
}