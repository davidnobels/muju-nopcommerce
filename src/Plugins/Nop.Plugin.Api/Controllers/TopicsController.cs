using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Api.Attributes;
using Nop.Plugin.Api.DTO.Errors;
using Nop.Plugin.Api.DTO.Topics;
using Nop.Plugin.Api.Helpers;
using Nop.Plugin.Api.JSON.Serializers;
using Nop.Plugin.Api.Services;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Services.Topics;
using Nop.Plugin.Api.Infrastructure;
using Nop.Plugin.Api.JSON.ActionResults;
using Nop.Plugin.Api.Models.TopicsParameters;
using System.Linq;

namespace Nop.Plugin.Api.Controllers
{
    public class TopicsController : BaseApiController
    {
        private readonly ITopicApiService _topicApiService;
        private readonly ITopicService _topicService;
        private readonly IDTOHelper _dtoHelper;
        private readonly IUrlRecordService _urlRecordService;

        public TopicsController(
            ITopicApiService topicApiService,
            IJsonFieldsSerializer jsonFieldsSerializer,
            ITopicService categoryService,
            IUrlRecordService urlRecordService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            IPictureService pictureService,
            IStoreMappingService storeMappingService,
            IStoreService storeService,
            IDiscountService discountService,
            IAclService aclService,
            ICustomerService customerService,
            IDTOHelper dtoHelper) : base(jsonFieldsSerializer, aclService, customerService, storeMappingService, storeService, discountService,
                                         customerActivityService, localizationService, pictureService)
        {
            _topicApiService = topicApiService;
            _topicService = categoryService;
            _urlRecordService = urlRecordService;
            _dtoHelper = dtoHelper;
        }

        /// <summary>
        ///     Receive a list of all Topics
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("/api/topics")]
        [ProducesResponseType(typeof(TopicsRootObject), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
        [GetRequestsErrorInterceptorActionFilter]
        public IActionResult GetCategories(TopicsParametersModel parameters)
        {
            if (parameters.Limit < Constants.Configurations.MinLimit || parameters.Limit > Constants.Configurations.MaxLimit)
            {
                return Error(HttpStatusCode.BadRequest, "limit", "Invalid limit parameter");
            }

            if (parameters.Page < Constants.Configurations.DefaultPageValue)
            {
                return Error(HttpStatusCode.BadRequest, "page", "Invalid page parameter");
            }

            var allTopics = _topicApiService.GetTopics(parameters.Ids, parameters.CreatedAtMin, parameters.CreatedAtMax,
                                                                  parameters.UpdatedAtMin, parameters.UpdatedAtMax,
                                                                  parameters.Limit, parameters.Page, parameters.SinceId)
                                                   .Where(c => StoreMappingService.Authorize(c));

            IList<TopicDto> topicsAsDtos = allTopics.Select(topic => _dtoHelper.PrepareTopicToDTO(topic)).ToList();

            var topicsRootObject = new TopicsRootObject
            {
                Topics = topicsAsDtos
            };

            var json = JsonFieldsSerializer.Serialize(topicsRootObject, parameters.Fields);

            return new RawJsonActionResult(json);
        }

        
    }
}
