namespace Aban360.UserPool.Application.Common.Base
{
    public interface IRequestHandler<in TRequest, TResponse> 
        //where TRequest : struct 
        //where TResponse : struct
    {
        public Task<TResponse> Handle(TRequest request);
    }

    public interface IRequestHandler<in TRequest>
    {
        public Task Handle(TRequest request);
    }
}
