using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShowLogger.Web.Data;

namespace ShowLogger.Web.Areas.Common;

public class BaseController : Controller
{
    public readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<BaseController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BaseController(
        UserManager<ApplicationUser> userManager,
        ILogger<BaseController> logger,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userManager = userManager;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

    public int GetLoggedInUserId()
    {
        ApplicationUser user = GetCurrentUserAsync().Result;

        if (user == null)
            return -1;
        else
            return user.UserId;
    }

    public IEnumerable<ModelErrorCollection> GetErrorsFromModelState()
    {
        return ModelState.Select(x => x.Value.Errors)
                       .Where(y => y.Count > 0)
                       .ToList();
    }

    public void HandleException(Exception ex, string message)
    {
        //StackTrace stackTrace = new StackTrace(ex);
        //string route = MySession.UserSession.Route;
        //IEnumerable<ParameterModel> parameters = MySession.UserSession.MethodParameters;
        //Tracing.LogMessage($"Error in method: {route}");

        //if (parameters != null)
        //{
        //    Tracing.LogMessage("Parameters are: ");
        //    foreach (var p in parameters)
        //    {
        //        if (p.Object.GetType().GetTypeInfo().IsClass && !p.Object.GetType().Equals(typeof(string)))
        //        {
        //            Tracing.LogMessage($"\t{p.ClassName}:");
        //            Type t = Type.GetType(p.FullyQualifiedName);
        //            object instance = Activator.CreateInstance(Type.GetType(p.FullyQualifiedName));
        //            instance = JsonConvert.DeserializeObject(p.Object.ToString(), t);

        //            IList<PropertyInfo> props = new List<PropertyInfo>(instance.GetType().GetProperties());
        //            foreach (PropertyInfo prop in props)
        //            {
        //                object propValue = prop.GetValue(instance, null);
        //                Tracing.LogMessage($"\t\t{prop.Name}: {propValue}");
        //            }
        //        }
        //        else
        //        {
        //            Tracing.LogMessage($"\t{p.ParameterName}: {p.Object}");
        //        }
        //    }
        //}

        //Tracing.LogMessage($"Message: {message}");

        //if (!string.IsNullOrEmpty(ex.Message))
        //{
        //    Tracing.LogMessage($"Exception Message: {ex.Message}");
        //}
    }
}
