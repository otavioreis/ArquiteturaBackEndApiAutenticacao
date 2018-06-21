using Swashbuckle.AspNetCore.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Livraria.Api.ObjectModel.Swagger.Examples
{
    public class LoginExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Login
            {
                Usuario = "otavio",
                Senha = "12345"
            };
        }
    }
}
