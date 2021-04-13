using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SampleAuthorizer
{
    public class Function
    {
        private const string EXECUTE_API_INVOKE = "execute-api:Invoke";
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayCustomAuthorizerResponse FunctionHandler(APIGatewayCustomAuthorizerRequest aPIGatewayCustomAuthorizerRequest, ILambdaContext context)
        {
            context.Logger.Log(JsonConvert.SerializeObject(aPIGatewayCustomAuthorizerRequest));
            var authPolicy = new APIGatewayCustomAuthorizerResponse();

            // attach policy document for resource policy to allow or deny.
            AddAuthPolicyDocument(authPolicy);

            //Custom

            var response = AuthResponse(authPolicy, "Deny", aPIGatewayCustomAuthorizerRequest.MethodArn);
            context.Logger.Log(JsonConvert.SerializeObject(response));
            return response;
        }

        private APIGatewayCustomAuthorizerPolicy.IAMPolicyStatement GetPolicy(string access, string methodArn)
        {
            var statement = new APIGatewayCustomAuthorizerPolicy.IAMPolicyStatement
            {
                Action = new HashSet<string>(new string[] { EXECUTE_API_INVOKE }),
                Effect = access,
                //statement.Resource = new HashSet<string>(new string[] { authorizationSettings.AuthorizerApiArn });
                Resource = new HashSet<string>(new string[] { methodArn })
            };
            return statement;
        }
        private void AddAuthPolicyDocument(APIGatewayCustomAuthorizerResponse authPolicy)
        {
            authPolicy.PrincipalID = "user";
            authPolicy.PolicyDocument = new APIGatewayCustomAuthorizerPolicy
            {
                Version = "2012-10-17",
                Statement = new List<APIGatewayCustomAuthorizerPolicy.IAMPolicyStatement>()
            };
        }

        private APIGatewayCustomAuthorizerResponse AuthResponse(APIGatewayCustomAuthorizerResponse authPolicyResponse, string access, string methodArn)
        {
            authPolicyResponse.PolicyDocument.Statement.Add(GetPolicy(access, methodArn));
           
            return authPolicyResponse;
        }
    }
}
