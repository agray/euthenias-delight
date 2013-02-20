using System.Xml;
using System.Xml.XPath;

namespace euthenias_delight.XML
{
    public class XmlDoc {
        public static XmlDocument Document { get; set; }
        public static XPathNavigator Navigator { get; set; }
        public static XPathNavigator ToNavItem { get; set; }
        public static XPathNavigator RootNavItem { get; set; }

        #region Properties

        public static string InvoiceTo { get { return GetValue(RootNavItem, INVOICE_TO); } }
        public static XPathNodeIterator InvoiceItems { get { return Navigator.Select(INVOICE_ITEMS); } }
        public static string InvoiceTerms { get { return GetValue(RootNavItem, INVOICE_TERMS); } }
        public static XPathNodeIterator InvoiceNotes { get { return Navigator.Select(INVOICE_NOTES); } }

        public static string CustomerNickname { get { return GetValue(ToNavItem, CUSTOMER_NICKNAME); } }
        public static string CustomerEmail { get { return GetValue(ToNavItem, CUSTOMER_EMAIL); } }
        public static string CustomerName { get { return GetValue(ToNavItem, CUSTOMER_NAME); } }
        public static string CustomerAddressLine1 { get { return GetValue(ToNavItem, CUSTOMER_ADDRESS_LINE1); } }
        public static string CustomerAddressCity { get { return GetValue(ToNavItem, CUSTOMER_ADDRESS_CITY); } }
        public static string CustomerAddressPostcode { get { return GetValue(ToNavItem, CUSTOMER_ADDRESS_POSTCODE); } }
        public static string CustomerAttention { get { return GetValue(ToNavItem, CUSTOMER_ATTENTION); } }

        public static double ItemQuantity(XPathNavigator item) { return GetValueAsDouble(item, ITEM_QUANTITY); }
        public static double ItemDiscount(XPathNavigator item) { return GetValueAsDouble(item, ITEM_DISCOUNT); }
        public static double ItemPrice(XPathNavigator item) { return GetValueAsDouble(item, ITEM_PRICE); }
        public static string ItemNumber(XPathNavigator item) { return GetValue(item, ITEM_NUMBER); }
        public static string ItemDescription(XPathNavigator item) { return GetValue(item, ITEM_DESCRIPTION); }

        public static string EmailBodyGreeting { get { return GetValue(RootNavItem, EMAILBODY_GREETING); } }
        public static string EmailBodyMessage { get { return GetValue(RootNavItem, EMAILBODY_MESSAGE); } }
        public static string EmailBodySalutation { get { return GetValue(RootNavItem, EMAILBODY_SALUTATION); } }

        public static double GSTPercent { get { return GetValueAsDouble(RootNavItem, GST_PERCENT); } }
        public static string GSTCollecting { get { return GetValue(RootNavItem, GST_COLLECTING); } }

        #endregion

        #region Selectors

        public const string ROOT = "/";

        public const string INVOICE_TO = "/invoice/to";
        public const string INVOICE_ITEMS = "/invoice/items/*";
        public const string INVOICE_TERMS = "/invoice/terms";
        public const string INVOICE_NOTES = "/invoice/notes/*";

        public const string CUSTOMER_NICKNAME = "/customer/nickname";
        public const string CUSTOMER_EMAIL = "customer/email";
        public const string CUSTOMER_NAME = "/customer/name/singleName";
        public const string CUSTOMER_ADDRESS_LINE1 = "/customer/address/line1";
        public const string CUSTOMER_ADDRESS_CITY = "/customer/address/city";
        public const string CUSTOMER_ADDRESS_POSTCODE = "/customer/address/postalCode";
        public const string CUSTOMER_ATTENTION = "/customer/attention";

        public const string ITEM_QUANTITY = "quantity";
        public const string ITEM_PRICE = "price";
        public const string ITEM_DISCOUNT = "discount";
        public const string ITEM_NUMBER = "itemNumber";
        public const string ITEM_DESCRIPTION = "description";

        public const string EMAILBODY_GREETING = "/invoice/emailBody/greeting";
        public const string EMAILBODY_MESSAGE = "/invoice/emailBody/message";
        public const string EMAILBODY_SALUTATION = "/invoice/emailBody/salutation";

        public const string GST_PERCENT = "invoice/gst/percent";
        public const string GST_COLLECTING = "invoice/gst/collecting";

        #endregion

        #region XML Accessors

        public static string GetValue(XPathNavigator item, string xPath) {
            XPathNavigator node = item.SelectSingleNode(xPath);
            return node != null ? node.ToString() : null;
        }

        public static double GetValueAsDouble(XPathNavigator item, string xPath) {
            return double.Parse(GetValue(item, xPath));
        }

        public static XPathNavigator SelectItem(string xPath) {
            XmlDocument subDoc = new XmlDocument();
            subDoc.LoadXml(XmlDoc.Document.SelectSingleNode(xPath).InnerXml);
            return subDoc.CreateNavigator();
        }

        #endregion
    }
}