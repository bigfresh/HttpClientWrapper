// See https://aka.ms/new-console-template for more information

using System.Collections.Specialized;
using ConsoleApp;
using HttpClientwrapper;

var queryParams = new NameValueCollection
{
    { "foo1", "1" },
    { "foo2", "20" },
    { "filter", "active" }
};
var rootObject = await new ApiClient().GetAsync<RootObject>("https://postman-echo.com/get",queryParams);

Console.WriteLine(rootObject.args.foo1);
