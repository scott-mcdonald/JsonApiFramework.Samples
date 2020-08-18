using System.Diagnostics.Contracts;

using Blogging.WebService.Framework;

namespace Blogging.WebService.Controllers
{
    public abstract class ApiControllerBase : ApiController
    {
        #region Constructors
        protected ApiControllerBase(IApiServiceContext apiServiceContext, BloggingRepository bloggingRepository)
            : base(apiServiceContext)
        {
            Contract.Requires(apiServiceContext != null);

            this.BloggingRepository = bloggingRepository;
        }
        #endregion

        #region Properties
        protected BloggingRepository BloggingRepository { get; }
        #endregion
    }
}