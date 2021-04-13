using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SampleLambda
{
    public class Function
    {
        public Function()
        {
            
        }
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            context.Logger.Log("Invoking Start");
            var allenvVar = Environment.GetEnvironmentVariables();
            var envVar = Environment.GetEnvironmentVariable("Sample");
            context.Logger.Log(envVar);
            string inputName = input.QueryStringParameters["input"];
            return new APIGatewayProxyResponse()
            {
                Body = inputName?.ToUpper(),
                StatusCode = 200,

            };
            
        }
    }
}
