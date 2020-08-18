using System.Diagnostics.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace Blogging.WebService.Framework
{
    public abstract class ApiController : ControllerBase
    {
        #region Constructors
        protected ApiController(IApiServiceContext apiServiceContext)
        {
            Contract.Requires(apiServiceContext != null);

            this.ApiServiceContext = apiServiceContext;
        }
        #endregion

        #region Properties
        protected IApiServiceContext ApiServiceContext { get; }
        #endregion
    }
}