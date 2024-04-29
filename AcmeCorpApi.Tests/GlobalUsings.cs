global using Xunit;

global using System.Text;
global using System.Net.Http.Headers;

global using AcmeCorpApi.Models;
global using AcmeCorpApi.Repository;
global using AcmeCorpApi.Utils.Extensions;
global using AcmeCorpApi.Utils.Generators;
global using AcmeCorpApi.Utils.Security;

global using Newtonsoft.Json;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.TestHost;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.EntityFrameworkCore;
global using Testcontainers.PostgreSql;