using Sitecore.Analytics.Model;
using Sitecore.Analytics.XConnect.DataAccess.Pipelines.ConvertToXConnectEventPipeline;
using Newtonsoft.Json;
using Sitecore.Commerce.AnalyticsData;
using Sitecore.Commerce.CustomModels;
using System.Collections.Generic;
using System.Globalization;
using Sitecore.XConnect;
using Sitecore.Commerce.CustomModels.PageEvents;

namespace Sitecore.Support.Commerce.CustomModels.PageEvents.Converters
{
    public class ConvertPageEventDataToVisitedProductDetailsPageEvent : ConverterBase
    {
        public ConvertPageEventDataToVisitedProductDetailsPageEvent() : base()
        {
        }

        public void PopulateEventFromJson(string pageEventJson, Event @event)
        {
            pageEventJson = this.CleanupMongoMigrationJson(pageEventJson);

            dynamic json = JsonConvert.DeserializeObject(pageEventJson);

            var pageEventData = new PageEventData();

            var data = AnalyticsDataInitializerFactory.Create<VisitedProductDetailsPageAnalyticsData>();

            Dictionary<string, object> customValuesDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.CustomValues.ToString());

            data.Deserialize(customValuesDict);

            this.PopulatePageEventFromBsonCustomValues(data, pageEventData);

            this.TranslateEvent(pageEventData, @event as VisitedProductDetailsPageEvent);
        }

        protected override bool CanProcessPageEventData(PageEventData pageEventData)
        {
            return pageEventData.PageEventDefinitionId == VisitedProductDetailsPageEvent.ID;
        }

        protected override Event CreateEvent(PageEventData pageEventData)
        {
            var pageEvent = new VisitedProductDetailsPageEvent(pageEventData.DateTime);

            this.TranslateEvent(pageEventData, pageEvent);

            return pageEvent;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        protected void TranslateEvent(PageEventData pageEventData, VisitedProductDetailsPageEvent pageEvent)
        {
            #region check has been added
            if (pageEventData.CustomValues.ContainsKey(Sitecore.Commerce.Constants.KnownPageEventDataNames.ShopName))
            {
                pageEvent.ShopName = pageEventData.CustomValues[Sitecore.Commerce.Constants.KnownPageEventDataNames.ShopName] as string;
            }


            if (pageEventData.CustomValues.ContainsKey(Sitecore.Commerce.Constants.KnownPageEventDataNames.ProductId))
            {
                pageEvent.ProductId = pageEventData.CustomValues[Sitecore.Commerce.Constants.KnownPageEventDataNames.ProductId] as string;
            }

            if (pageEventData.CustomValues.ContainsKey(Sitecore.Commerce.Constants.KnownPageEventDataNames.ProductName))
            {
                pageEvent.ProductName = pageEventData.CustomValues[Sitecore.Commerce.Constants.KnownPageEventDataNames.ProductName] as string;
            }


            if (pageEventData.CustomValues.ContainsKey(Sitecore.Commerce.Constants.KnownPageEventDataNames.ParentCategoryId))
            {
                pageEvent.ParentCategoryId = pageEventData.CustomValues[Sitecore.Commerce.Constants.KnownPageEventDataNames.ParentCategoryId] as string;
            }


            if (pageEventData.CustomValues.ContainsKey(Sitecore.Commerce.Constants.KnownPageEventDataNames.ParentCategoryName))
            {
                pageEvent.ParentCategoryName = pageEventData.CustomValues[Sitecore.Commerce.Constants.KnownPageEventDataNames.ParentCategoryName] as string;
            }

            if (pageEventData.CustomValues.ContainsKey(Sitecore.Commerce.Constants.KnownPageEventDataNames.Currency))
            {
                pageEvent.CurrencyCode = pageEventData.CustomValues[Sitecore.Commerce.Constants.KnownPageEventDataNames.Currency] as string;
            }

            if (pageEventData.CustomValues.ContainsKey(Sitecore.Commerce.Constants.KnownPageEventDataNames.Amount))
            {
                pageEvent.Amount = System.Convert.ToDecimal(pageEventData.CustomValues[Sitecore.Commerce.Constants.KnownPageEventDataNames.Amount], CultureInfo.InvariantCulture);
            }
            #endregion
        }
    }
}
