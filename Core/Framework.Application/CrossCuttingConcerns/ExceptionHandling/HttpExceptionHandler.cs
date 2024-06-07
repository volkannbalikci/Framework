using Framework.Application.Exceptions.Auth;
using Framework.Application.Exceptions.BusinessLogic;
using Framework.Application.Exceptions.Database;
using Framework.Application.Exceptions.InternalServer;
using Framework.Application.Responses.ExceptionResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Framework.Application.Constants;
using Framework.Application.Exceptions.Validation;
using Framework.Application.Exceptions.Common;

namespace Framework.Application.CrossCuttingConcerns.ExceptionHandling
{
    public class HttpExceptionHandler
    {
        private readonly RequestDelegate _next;

        public HttpExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        private String _requestedMethodName { get; set; }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = HttpContextConstants.ContentType;
            var requestPathElements = httpContext.Request.Path.Value.Split("/");
            _requestedMethodName = requestPathElements.Last();

            switch (exception.GetType().Name)
            {
                case var value when value == typeof(BusinessLogicException).Name:
                    await HandleBusinessLogicExceptionAsync(httpContext, exception);
                    break;
                case var value when value == typeof(AuthenticationException).Name:
                    await HandleAuthenticationExceptionAsync(httpContext, exception);
                    break;
                case var value when value == typeof(AuthorizationException).Name:
                    await HandleAuthorizationExceptionAsync(httpContext, exception);
                    break;
                case var value when value == typeof(DatabaseException).Name:
                    await HandleDatabaseExceptionAsync(httpContext, exception);
                    break;
                case var value when value == typeof(InternalServerException).Name:
                    await HandleInternalServerExceptionAsync(httpContext, exception);
                    break;
                default:
                    await HandleCustomExceptionAsync(httpContext, exception);
                    break;
            }
        }

        private async Task HandleBusinessLogicExceptionAsync(HttpContext httpContext, Exception exception)
        {

            httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

            BusinessLogicException businessLogicException = (BusinessLogicException)exception; // iş hatası: ürün eklenemedi: addProduct

            //if (true)
            //{
            //    throw new BusinessLogicException("hata", "Ürün Alınamadı", "Ürün Satın Alma");
            //    exceptin.message -> "hata:ürün alınamadı:ürün satın alma"
            //}

            String[] exceptionHeaders = businessLogicException.Message.Split(PunctuationConstants.Colon);

            BusinessLogicExceptionDetails exceptionDetails = new BusinessLogicExceptionDetails(exceptionHeaders[2]);

            ExceptionResponse exceptionResponse = new ExceptionResponse(businessLogicException, exceptionDetails, exceptionHeaders[0], exceptionHeaders[1], HttpStatusCode.BadRequest);

            await httpContext.Response.WriteAsync(exceptionResponse.GetDetails());
        }

        private async Task HandleAuthenticationExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);

            AuthenticationException authenticationException = (AuthenticationException)exception;

            String[] exceptionHeaders = authenticationException.Message.Split(PunctuationConstants.Colon);

            AuthenticationExceptionDetails exceptionDetails = new AuthenticationExceptionDetails();

            ExceptionResponse exceptionResponse = new ExceptionResponse(authenticationException, exceptionDetails, exceptionHeaders[0], exceptionHeaders[1], HttpStatusCode.BadRequest);

            await httpContext.Response.WriteAsync(exceptionResponse.GetDetails());
        }

        private async Task HandleAuthorizationExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Forbidden);

            AuthorizationException authorizationException = (AuthorizationException)exception;

            String[] exceptionHeaders = authorizationException.Message.Split(PunctuationConstants.Colon);

            AuthorizationExceptionDetails exceptionDetails = new AuthorizationExceptionDetails(exceptionHeaders[2]);

            ExceptionResponse exceptionResponse = new ExceptionResponse(authorizationException, exceptionDetails, exceptionHeaders[0], exceptionHeaders[1], HttpStatusCode.BadRequest);

            await httpContext.Response.WriteAsync(exceptionResponse.GetDetails());
        }

        private async Task HandleDatabaseExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);

            DatabaseException databaseException = (DatabaseException)exception;

            String[] exceptionHeaders = databaseException.Message.Split(PunctuationConstants.Colon);

            DatabaseExceptionDetails exceptionDetails = new DatabaseExceptionDetails(exceptionHeaders[2]);

            ExceptionResponse exceptionResponse = new ExceptionResponse(databaseException, exceptionDetails, exceptionHeaders[0], exceptionHeaders[1], HttpStatusCode.BadRequest);

            await httpContext.Response.WriteAsync(exceptionResponse.GetDetails());
        }

        private async Task HandleInternalServerExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);

            InternalServerException internalServerException = (InternalServerException)exception;

            String[] exceptionHeaders = internalServerException.Message.Split(PunctuationConstants.Colon);

            InternalServerExceptionDetails exceptionDetails = new InternalServerExceptionDetails(exceptionHeaders[2]);

            ExceptionResponse exceptionResponse = new ExceptionResponse(internalServerException, exceptionDetails, exceptionHeaders[0], exceptionHeaders[1], HttpStatusCode.BadRequest);

            await httpContext.Response.WriteAsync(exceptionResponse.GetDetails());
        }

        private async Task HandleValidationExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

            var validationException = (FluentValidation.ValidationException)exception;

            var validationErrors = validationException.Errors;

            var exceptionDetails = new ValidationExceptionDetails(validationErrors);

            ExceptionResponse exceptionResponse = new ExceptionResponse(validationException, exceptionDetails, ResponseTitleConstants.Error, ResponseMessageConstants.ValidationErrorMessage, HttpStatusCode.BadRequest);

            await httpContext.Response.WriteAsync(exceptionResponse.GetDetails());
        }

        private async Task HandleCustomExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

            ExceptionResponse exceptionResponse = new ExceptionResponse(exception, new ExceptionDetailsBase() { ThrownDate = DateTime.Now }, exception.Source, exception.Message, HttpStatusCode.BadRequest);

            await httpContext.Response.WriteAsync(exceptionResponse.GetDetails());
        }
    }
}
