using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace BVSoftware.Web.Geography
{
    public class Country
    {

        public const string UnitedStatesCountryBvin = "bf7389a2-9b21-4d33-b276-23c9c18ea0c0";

        // Variables    
        private string _Bvin = string.Empty;
        private string _CultureCode = string.Empty;
        private string _DisplayName = string.Empty;
        private string _IsoCode = string.Empty;
        private string _IsoAlpha3 = string.Empty;
        private string _IsoNumeric = string.Empty;
        private List<Region> _Regions = null;
        private string _PostalCodeValidationRegex = string.Empty;
        private string _USPostalServiceName = string.Empty;

        // Properties        
        public string Bvin
        {
            get { return _Bvin; }
            set { _Bvin = value; }
        }
        public string CultureCode
        {
            get { return _CultureCode; }

            set { _CultureCode = value; }
        }
        public string DisplayName
        {
            get { return _DisplayName; }

            set { _DisplayName = value; }
        }
        public string IsoCode
        {
            get { return _IsoCode; }
            set { _IsoCode = value; }
        }
        public string IsoAlpha3
        {
            get { return _IsoAlpha3; }
            set { _IsoAlpha3 = value; }
        }
        public string IsoNumeric
        {
            get { return _IsoNumeric; }
            set { _IsoNumeric = value; }
        }
        public List<Region> Regions
        {
            get
            {
                if (_Regions == null)
                {
                    LoadRegions();
                }
                return _Regions;
            }
        }
        public Region FindRegion(string abbreviation)
        {
            Region result = null;
            result = Regions.Where(y => y.Abbreviation == abbreviation).SingleOrDefault();
            return result;
        }
        public string USPostalServiceName
        {
            get { return _USPostalServiceName; }
            set { _USPostalServiceName = value; }
        }

        public string SampleCurrency
        {
            get
            {
                string result = string.Empty;

                try
                {
                    System.Globalization.CultureInfo tempCulture;
                    tempCulture = System.Globalization.CultureInfo.CreateSpecificCulture(_CultureCode);
                    if (tempCulture != null)
                    {
                        result = string.Format(tempCulture, "{0:c}", 1.23);
                    }
                }
                catch (Exception ex)
                {

                }

                return result;
            }
        }
        public string SampleNameAndCurrency
        {
            get
            {
                string result = string.Empty;
                result = DisplayName + " " + SampleCurrency;
                return result;
            }
        }
        public string PostalCodeValidationRegex
        {
            get { return _PostalCodeValidationRegex; }
            set
            {
                if (_PostalCodeValidationRegex == value)
                {
                    return;
                }
                _PostalCodeValidationRegex = value;
            }
        }

        // Methods                
        public Country()
        {
        }
        public Country(string bvin, string cultureCode, string displayName,
                      string iso2, string iso3, string isoNumber,
                      string postalcodeRegEx)
        {
            this.Bvin = bvin;
            this.CultureCode = cultureCode;
            this.DisplayName = displayName;
            this.IsoCode = iso2;
            this.IsoAlpha3 = iso3;
            this.IsoNumeric = isoNumber;
            this.PostalCodeValidationRegex = postalcodeRegEx;
            this.USPostalServiceName = this.DisplayName;
        }
        public Country(string bvin, string cultureCode, string displayName,
                     string iso2, string iso3, string isoNumber,
                     string postalcodeRegEx, string usPostalServiceName)
        {
            this.Bvin = bvin;
            this.CultureCode = cultureCode;
            this.DisplayName = displayName;
            this.IsoCode = iso2;
            this.IsoAlpha3 = iso3;
            this.IsoNumeric = isoNumber;
            this.PostalCodeValidationRegex = postalcodeRegEx;
            this.USPostalServiceName = usPostalServiceName;
        }

        public bool ValidatePostalCode(string postalCode)
        {
            if (!string.IsNullOrEmpty(this.PostalCodeValidationRegex))
            {
                return System.Text.RegularExpressions.Regex.IsMatch(postalCode, this.PostalCodeValidationRegex);
            }
            else
            {
                return true;
            }
        }

        public static Country FindByBvin(string bvin)
        {
            var c = (Country)FindAll().Where(y => y.Bvin == bvin).SingleOrDefault();
            if (c != null)
                return c;

            return new Country();
        }

        public static List<Country> FindAllExceptList(List<string> disabledIso3Codes)
        {
            List<Country> result = FindAll().Where(y => !(disabledIso3Codes.Contains(y.IsoAlpha3.Trim().ToLowerInvariant()))).ToList();
            return result;
        }
        public static List<Country> FindAllInList(List<string> matchingIso3Codes)
        {
            List<Country> result = FindAll().Where(y => matchingIso3Codes.Contains(y.IsoAlpha3.Trim().ToLowerInvariant())).ToList();
            return result;
        }


        /*
// Possible Missing Countries ---

            //Afghanistan 237
//Andorra 247
//Angola 250
//Anguilla 253
//Antigua and Barbuda 256
//Aruba 266
//Ascension 269
//Bahamas 282
//Bangladesh 288
//Barbados 292
//Benin 305
//Bermuda 308
//Bhutan 311
//Bosnia-Herzegovina 317
//Botswana 321
//British Virgin Islands 328
//Burkina Faso 337
//Burma 341
//Burundi 343
//Cambodia 346
//Cameroon 349
//Cape Verde 357
//Cayman Islands 360
//Central African Republic 363
//Chad 366
//Comoros 379
//Congo, Democratic Republic of the 381
//Congo, Republic of the 384
//Cote d’Ivoire 390
//Cuba 396
//Cyprus 400
//Djibouti 410
//Dominica 413
//Equatorial Guinea 429
//Eritrea 432
//Ethiopia 439
//Falkland Islands 442
//Fiji 447
//French Guiana 458
//French Polynesia 461
//Gabon 465
//Gambia 468
//Ghana 478
//Gibraltar 481
//Great Britain and Northern Ireland 484
//Greenland 492
//Grenada 495
//Guadeloupe 498
//Guinea 504
//Guinea–Bissau 507
//Guyana 510
//Haiti 513
//Kiribati 574
//Kosovo, Republic of 581
//Kyrgyzstan 587
//Laos 591
//Lesotho 600
//Liberia 603
//Madagascar 627
//Malawi 630
//Mali 639
//Malta 642
//Martinique 646
//Mauritania 649
//Mauritius 652
//Moldova 658
//Montenegro 666
//Montserrat 669
//Mozambique 675
//Namibia 678
//Nauru 681
//Nepal 684
//New Caledonia 694
//Niger 703
//Nigeria 706
//Papua New Guinea 723
//Pitcairn Island 737
//Reunion 749
//Rwanda 759
//Saint Christopher and Nevis 762
//Saint Helena 766
//Saint Lucia 768
//Saint Pierre and Miquelon 771
//Saint Vincent and the Grenadines 773
//San Marino 776
//Sao Tome and Principe 780
//Senegal 786
//Seychelles 793
//Sierra Leone 796
//Solomon Islands 809
//Somalia 812
//Sri Lanka 820
//Sudan 823
//Suriname 826
//Swaziland 829
//Tajikistan 846
//Tanzania 850
//Togo 857
//Tonga 860
//Tristan da Cunha 866
//Turkmenistan 875
//Turks and Caicos Islands 879
//Tuvalu 882
//Uganda 884
//Vanuatu 900
//Vatican City 903
//Wallis and Futuna Islands 914
//Western Samoa 917
            //Zambia
            */
        public static List<Country> FindAll()
        {
            List<Country> result = new List<Country>();
            
            result.Add(new Country("2c6d4fa9-941d-4d5d-807f-7bccba497680", "sq-AL", "Albania", "AL", "ALB", "008", "", "Albania"));
            result.Add(new Country("255b01ab-4d4f-40eb-b63d-e86ed8f7bd13", "ar-DZ", "Algeria", "DZ", "DZA", "012", "", "Algeria"));
            result.Add(new Country("69135901-5cda-42b3-9bb9-eb0318c08ef5", "es-AR", "Argentina", "AR", "ARG", "032", "", "Argentina"));
            result.Add(new Country("1e4fadd8-5b90-4534-8220-6be963044f39", "hy-AM", "Armenia", "AM", "ARM", "051", "", "Armenia"));
            result.Add(new Country("cb87aef1-ae12-4363-a292-8cff1a4e4d02", "en-AU", "Australia", "AU", "AUS", "036", "", "Australia"));
            result.Add(new Country("6594806a-8e9e-4424-ac4b-4ffa19adb683", "de-AT", "Austria", "AT", "AUT", "040", "", "Austria"));
            result.Add(new Country("831a08f8-f3bd-486a-8af5-d53bc2db69fb", "az-AZ-Latn", "Azerbaijan", "AZ", "AZE", "031", "", "Azerbaijan"));
            result.Add(new Country("8f6413ef-2edd-4d6c-a9d8-40257bb9dca4", "ar-BH", "Bahrain", "BH", "BHR", "048", "", "Bahrain"));
            result.Add(new Country("6d37454c-7e9b-4a8c-981c-acc656cbb542", "be-BY", "Belarus", "BY", "BLR", "112", "", "Belarus"));
            result.Add(new Country("d6764c27-b7b3-49bd-88fb-1385570ad5ef", "fr-BE", "Belgium", "BE", "BEL", "056", "", "Belgium"));
            result.Add(new Country("6926f9ef-ccf2-43e4-9c3f-2b0477f4998e", "en-BZ", "Belize", "BZ", "BLZ", "084", "", "Belize"));
            result.Add(new Country("e911f5fb-157d-41ed-84d6-f7c23ac053cf", "es-BO", "Bolivia", "BO", "BOL", "068", "", "Bolivia"));
            result.Add(new Country("33ac233a-dc15-4aef-89db-7beda61b0436", "pt-BR", "Brazil", "BR", "BRA", "076", "", "Brazil"));
            result.Add(new Country("254a6f90-f4dd-4218-879e-9869c38f9236", "ms-BN", "Brunei Darussalam", "BN", "BRN", "096", "", "Brunei Darussalam"));
            result.Add(new Country("c7cb9d92-9ca1-431e-8e27-e5953b895d92", "bg-BG", "Bulgaria", "BG", "BGR", "100", "", "Bulgaria"));
            result.Add(new Country("94052dcf-1ac8-4b65-813b-b17b12a0491f", "en-CA", "Canada", "CA", "CAN", "124", "", "Canada"));
            result.Add(new Country("292cc30a-8976-480f-8105-1e12aa128b2f", "en-CB", "Caribbean", "CB", "", "", ""));
            result.Add(new Country("a863ac16-e89c-46cc-bfd4-d518843445b7", "es-CL", "Chile", "CL", "CHL", "152", "", "Chile"));
            result.Add(new Country("9046b9d5-9bb9-43f5-b605-e7164f99d7d9", "zh-CN", "China", "CN", "CHN", "156", "", "China"));
            result.Add(new Country("817edac5-daf0-4722-b02e-03e91309f5fe", "es-CO", "Colombia", "CO", "COL", "170", "", "Colombia"));
            result.Add(new Country("83cea890-c1de-41be-a213-635d1416d6db", "es-CR", "Costa Rica", "CR", "CRI", "188", "", "Costa Rica"));
            result.Add(new Country("be57e8d8-6a09-4589-9181-d8558d27f4b5", "hr-HR", "Croatia", "HR", "HRV", "191", "", "Croatia"));
            result.Add(new Country("d2b92f8b-8645-433d-9f6d-63a34d895798", "cs-CZ", "Czech Republic", "CZ", "CZE", "203", "", "Czech Republic"));
            result.Add(new Country("1f54446f-f586-483a-9d7d-7eeb812f248c", "da-DK", "Denmark", "DK", "DNK", "208", "", "Denmark"));
            result.Add(new Country("ed004593-31b7-4c6b-b918-8e7efdb9c8e6", "es-DO", "Dominican Republic", "DO", "DOM", "214", "", "Dominican Republic"));
            result.Add(new Country("424dd0e3-3cf3-4d46-b13b-2ea43da8c8ac", "es-EC", "Ecuador", "EC", "ECU", "218", "", "Ecuador"));
            result.Add(new Country("99d0a3c3-4b61-4979-8fde-52584110392e", "ar-EG", "Egypt", "EG", "EGY", "818", "", "Egypt"));
            result.Add(new Country("c2a2f20d-821c-4573-ab4d-349a8c858de7", "es-SV", "El Salvador", "SV", "SLV", "222", "", "El Salvador"));
            result.Add(new Country("678de6b8-dcbb-4fcf-ac27-6c6f72dd5291", "et-EE", "Estonia", "EE", "EST", "233", "", "Estonia"));
            result.Add(new Country("b8f7c10f-8274-4642-a828-8b41946ac95d", "fo-FO", "Faroe Islands", "FO", "FRO", "234", "", "Faroe Islands"));
            result.Add(new Country("cd884fa0-4d0b-41d9-b45d-0a486fd71009", "fi-FI", "Finland", "FI", "FIN", "246", "", "Finland"));
            result.Add(new Country("1c076e3d-43d5-4b8f-bf8c-6946fb58d231", "fr-FR", "France", "FR", "FRA", "250", "", "France"));
            result.Add(new Country("68249d05-62f5-45c9-9735-4057bf0dee7e", "ka-GE", "Georgia", "GE", "GEO", "268", "", "Georgia, Republic of"));
            result.Add(new Country("8d447e0d-6327-4a05-a7e5-f50fc408e13c", "de-DE", "Germany", "DE", "DEU", "276", "", "Germany"));
            result.Add(new Country("225a471b-8c5c-4b86-9a2d-ea443919eb55", "el-GR", "Greece", "GR", "GRC", "300", "", "Greece"));
            result.Add(new Country("d11db563-7496-4eb5-88d1-81f20bbfbe8c", "es-GT", "Guatemala", "GT", "GTM", "320", "", "Guatemala"));
            result.Add(new Country("6aa3072c-540f-4e07-b203-de21180a94bd", "es-HN", "Honduras", "HN", "HND", "340", "", "Honduras"));
            result.Add(new Country("67ce5007-5a16-47b0-bd85-173a8a758944", "zh-HK", "Hong Kong", "HK", "HKG", "344", "", "Hong Kong"));
            result.Add(new Country("acf84f60-6b00-4131-a5be-fa202f1eb569", "hu-HU", "Hungary", "HU", "HUN", "348", "", "Hungary"));
            result.Add(new Country("4af15981-2ffe-44ff-8c18-7680851ea335", "is-IS", "Iceland", "IS", "ISL", "352", "", "Iceland"));
            result.Add(new Country("fa10d66f-3a6a-4b3b-980a-cc66143a2328", "hi-IN", "India", "IN", "IND", "356", "", "India"));
            result.Add(new Country("b698900d-71ca-43b3-977c-95d04c152141", "id-ID", "Indonesia", "ID", "IDN", "360", "", "Indonesia"));
            result.Add(new Country("f01b4b56-e99c-4b5d-b10e-29be58a79e9b", "fa-IR", "Iran", "IR", "IRN", "364", "", "Iran"));
            result.Add(new Country("85a3f763-9543-4a0a-921b-f71a36e3b811", "ar-IQ", "Iraq", "IQ", "IRQ", "368", "", "Iraq"));
            result.Add(new Country("a4ac08ae-548a-48bb-af12-d2167c26b673", "en-IE", "Ireland", "IE", "IRL", "372", "", "Ireland"));
            result.Add(new Country("864722db-70ae-4078-b80c-b60b5216b020", "he-IL", "Israel", "IL", "ISR", "376", "", "Israel"));
            result.Add(new Country("ceb2ea09-1d36-4b44-ac7c-c5d7660ef4f9", "it-IT", "Italy", "IT", "ITA", "380", "", "Italy"));
            result.Add(new Country("04a2314b-66a2-4157-b17c-0d848b73e9c6", "en-JM", "Jamaica", "JM", "JAM", "388", "", "Jamaica"));
            result.Add(new Country("b430b323-926d-4f1a-ac22-eb1b1479fcf6", "ja-JP", "Japan", "JP", "JPN", "392", "", "Japan"));
            result.Add(new Country("6f609d0b-e837-4f43-bd3b-a6f4b631942f", "ar-JO", "Jordan", "JO", "JOR", "400", "", "Jordan"));
            result.Add(new Country("f7867666-bb5e-46fe-9f49-39fdc0703382", "kk-KZ", "Kazakhstan", "KZ", "KAZ", "398", "", "Kazakhstan"));
            result.Add(new Country("3c3c25d5-0067-49f2-b951-22877d41bbb4", "sw-KE", "Kenya", "KE", "KEN", "404", "", "Kenya"));
            result.Add(new Country("be09cdcc-73c5-4829-85a1-ef99a480add8", "ar-KW", "Kuwait", "KW", "KWT", "414", "", "Kuwait"));
            result.Add(new Country("3335935f-84fa-4dcd-9a0b-fa4d6506d2f3", "lv-LV", "Latvia", "LV", "LVA", "428", "", "Latvia"));
            result.Add(new Country("2a330eef-a115-452a-a78d-d94a8d8b7da7", "ar-LB", "Lebanon", "LB", "LBN", "422", "", "Lebanon"));
            result.Add(new Country("dc3b811a-83f5-4f89-a998-af5abd015109", "ar-LY", "Libya", "LY", "LBY", "434", "", "Libya"));
            result.Add(new Country("d3e3c48a-c2e9-4c91-98d0-bf2f7802d6d6", "de-LI", "Liechtenstein", "LI", "LIE", "438", "", "Liechtenstein"));
            result.Add(new Country("83bfd137-925d-4b64-9559-b6aa9baf9085", "lt-LT", "Lithuania", "LT", "LTU", "440", "", "Lithuania"));
            result.Add(new Country("0f812320-3aaf-4f3e-96ff-01396751cca6", "fr-LU", "Luxembourg", "LU", "LUX", "442", "", "Luxembourg"));
            result.Add(new Country("815902dd-3392-4dc1-b148-3ddcb1a5828c", "zh-MO", "Macau", "MO", "MAC", "446", "", "Macao"));
            result.Add(new Country("01e68cef-1c77-4c2a-bb7b-5295a71379ad", "mk-MK", "Macedonia", "MK", "MKD", "807", "", "Macedonia, Republic of"));
            result.Add(new Country("deddf780-661e-41f7-b5aa-a539cd21e1dd", "ms-MY", "Malaysia", "MY", "MYS", "458", "", "Malaysia"));
            result.Add(new Country("271f61fc-9f93-4ac2-bcda-d62640e23489", "div-MV", "Maldives", "MV", "MDV", "462", "", "Maldives"));
            result.Add(new Country("cb2f2e2c-459b-4370-a077-7831fdd16733", "es-MX", "Mexico", "MX", "MEX", "484", "", "Mexico"));
            result.Add(new Country("1a109d87-e02c-4ac9-b4c9-a4f74e802ebb", "fr-MC", "Monaco", "MC", "MCO", "492", "", "Monaco (France)"));
            result.Add(new Country("3dc123db-aa7b-4223-a37a-793d84b35468", "mn-MN", "Mongolia", "MN", "MNG", "496", "", "Mongolia"));
            result.Add(new Country("1118faf0-bb14-41d4-9848-cc011f1a344a", "ar-MA", "Morocco", "MA", "MAR", "504", "", "Morocco"));
            result.Add(new Country("d6ae4091-0de6-4fb6-b6f9-99e4f7d3a96a", "nl-NL", "Netherlands", "NL", "NLD", "528", "", "Netherlands"));
            result.Add(new Country("2ebf498d-e6d6-4814-b5f9-f152afcac264", "en-NZ", "New Zealand", "NZ", "NZL", "554", "", "New Zealand"));
            result.Add(new Country("6c866a94-5c9f-414b-b10d-64e8dd330761", "es-NI", "Nicaragua", "NI", "NIC", "558", "", "Nicaragua"));
            result.Add(new Country("57E083A5-43B6-47f5-90AF-E7153794F0C9", "ko-KR", "North Korea", "KR", "KOR", "410", "", "North Korea (Korea, Democratic People’s Republic of)"));
            result.Add(new Country("39c7e72b-1721-451c-a2f7-476fcc20aa72", "nb-NO", "Norway", "NO", "NOR", "578", "", "Norway"));
            result.Add(new Country("aafcbe32-4e5a-4a8c-b101-699274072571", "ar-OM", "Oman", "OM", "OMN", "512", "", "Oman"));
            result.Add(new Country("e750df18-7cee-42b2-a334-ba82f26f3dd8", "ur-PK", "Pakistan", "PK", "PAK", "586", "", "Pakistan"));
            result.Add(new Country("52b52f9e-a2c8-4e0d-92eb-678df74f921e", "es-PA", "Panama", "PA", "PAN", "591", "", "Panama"));
            result.Add(new Country("e606fc92-c7a5-4bd5-abd4-f220db830ee2", "es-PY", "Paraguay", "PY", "PRY", "600", "", "Paraguay"));
            result.Add(new Country("acff76b1-cdd4-4c3e-872e-d655fb092a29", "es-PE", "Peru", "PE", "PER", "604", "", "Peru"));
            result.Add(new Country("3c707cc5-8a8c-4be9-abd1-3dabdce6dd3f", "en-PH", "Philippines", "PH", "PHL", "608", "", "Philippines"));
            result.Add(new Country("f6eb3a3f-ea89-43e3-90d4-b665d382542b", "pl-PL", "Poland", "PL", "POL", "616", "", "Poland"));
            result.Add(new Country("cb043623-5166-481b-bd1c-79f4f7a80b16", "pt-PT", "Portugal", "PT", "PRT", "620", "", "Portugal"));
            result.Add(new Country("956066df-b065-43f0-9a2e-85eed038717e", "es-PR", "Puerto Rico", "PR", "PRI", "630", ""));
            result.Add(new Country("1b5a4f35-08a6-4259-b375-c4416bec5f11", "ar-QA", "Qatar", "QA", "QAT", "634", "", "Qatar"));
            result.Add(new Country("7ab0fab9-7605-45cc-a20a-3e59e0b6151f", "ro-RO", "Romania", "RO", "ROU", "642", "", "Romania"));
            result.Add(new Country("6b6706b6-11ad-4843-bae6-bf8de441749a", "ru-RU", "Russia", "RU", "RUS", "643", "", "Russia"));
            result.Add(new Country("c636ea8b-a773-48df-b5dc-00bf3b8bef87", "ar-SA", "Saudi Arabia", "SA", "SAU", "682", "", "Saudi Arabia"));
            result.Add(new Country("d5e8ed18-b909-4fcf-b6c6-b7a7e00a28d4", "sr-SP-Latn", "Serbia", "RS", "SRB", "688", "", "Serbia, Republic of"));
            result.Add(new Country("226c5e05-3e46-4129-9594-268d156aaffa", "zh-SG", "Singapore", "SG", "SGP", "702", "", "Singapore"));
            result.Add(new Country("160dbdd9-9957-4bc2-8385-201c3bf32ea0", "sk-SK", "Slovakia", "SK", "SVK", "703", "", "Slovak Republic (Slovakia)"));
            result.Add(new Country("727971df-2751-4e7f-9b22-5f4097241aa7", "sl-SI", "Slovenia", "SI", "SVN", "705", "", "Slovenia"));
            result.Add(new Country("bfff7d8f-0beb-44d5-bd9c-745b7e939e83", "en-ZA", "South Africa", "ZA", "ZAF", "710", "", "South Africa"));
            result.Add(new Country("bd87cd0a-2470-4024-b8f2-bab111899375", "ko-KR", "South Korea", "KR", "KOR", "410", "", "South Korea (Korea, Republic of)"));
            result.Add(new Country("1cf4480c-5ef8-4a80-a43a-600a69ff2157", "es-ES", "Spain", "ES", "ESP", "724", "", "Spain"));
            result.Add(new Country("90951a60-ed1a-475d-be9d-e11982593326", "sv-SE", "Sweden", "SE", "SWE", "752", "", "Sweden"));
            result.Add(new Country("ebb0cd59-d227-4c38-b31b-c6c93332bdde", "fr-CH", "Switzerland", "CH", "CHE", "756", "", "Switzerland"));
            result.Add(new Country("d53313a7-cc06-4b09-a56e-9715d32aaf02", "ar-SY", "Syrian Arab Republic", "SY", "SYR", "760", "", "Syrian Arab Republic (Syria)"));
            result.Add(new Country("37ba6822-e99f-430a-a3ec-e665ac1a3c8d", "zh-TW", "Taiwan", "TW", "TWN", "158", "", "Taiwan"));
            result.Add(new Country("443e409d-0daf-413a-ac06-706375aa0775", "th-TH", "Thailand", "TH", "THA", "764", "", "Thailand"));
            result.Add(new Country("62b52f42-fe5e-4329-8c40-8483ff1a6996", "en-TT", "Trinidad and Tobago", "TT", "TTO", "780", "", "Trinidad and Tobago"));
            result.Add(new Country("dfd415ae-d8e9-42b4-a143-d1bd6874efe3", "ar-TN", "Tunisia", "TN", "TUN", "788", "", "Tunisia"));
            result.Add(new Country("62f5a76b-622a-4611-9ad6-17465b56b816", "tr-TR", "Turkey", "TR", "TUR", "792", "", "Turkey"));
            result.Add(new Country("f23ea508-4df9-4aba-9b79-a3de288aa0b2", "ar-AE", "U.A.E.", "AE", "ARE", "784", "", "United Arab Emirates"));
            result.Add(new Country("f3e85660-e4a1-4dc8-afaf-4b4d0a0043f9", "uk-UA", "Ukraine", "UA", "UKR", "804", "", "Ukraine"));
            result.Add(new Country("9c7a73b1-7ef5-4fc2-beeb-ce4c70cca32f", "en-GB", "United Kingdom", "GB", "GBR", "826", "", "United Kingdom (Great Britain and Northern Ireland)"));
            result.Add(new Country("bf7389a2-9b21-4d33-b276-23c9c18ea0c0", "en-US", "United States", "US", "USA", "840", "", "United States"));
            result.Add(new Country("58b3af90-cdbd-4d36-a56e-5a4a2f6d7dbe", "es-UY", "Uruguay", "UY", "URY", "858", "", "Uruguay"));
            result.Add(new Country("81cd3640-c7ce-483f-b40c-fee3ee6a3a39", "uz-UZ-Latn", "Uzbekistan", "UZ", "UZB", "860", "", "Uzbekistan"));
            result.Add(new Country("4787bcae-97ce-46ac-aedf-fc5bbc69e509", "es-VE", "Venezuela", "VE", "VEN", "862", "", "Venezuela"));
            result.Add(new Country("d7c507f0-a613-418f-8011-df0d380d8835", "vi-VN", "Viet Nam", "VN", "VNM", "704", "", "Vietnam"));
            result.Add(new Country("a5ffd15d-731e-4d9d-b9c8-6f68cbffe625", "ar-YE", "Yemen", "YE", "YEM", "887", "", "Yemen"));
            result.Add(new Country("0d813c30-067a-4420-b159-84eb8c98b7cb", "en-ZW", "Zimbabwe", "ZW", "ZWE", "716", "", "Zimbabwe"));

            return result;
        }
        public static Country FindByISOCode(string isoCode)
        {
            var c = (Country)FindAll().Where(y => y.IsoCode.Trim().ToLowerInvariant() == isoCode.Trim().ToLowerInvariant()
                                                    || y.IsoAlpha3.Trim().ToLowerInvariant() == isoCode.Trim().ToLowerInvariant()
                                                    || y.IsoNumeric.Trim().ToLowerInvariant() == isoCode.Trim().ToLowerInvariant()).SingleOrDefault();
            if (c != null)
                return c;

            return new Country();
        }
        public static Country FindByName(string name)
        {
            var c = (Country)FindAll().Where(y => y.DisplayName.Trim().ToLowerInvariant() == name.Trim().ToLowerInvariant()).SingleOrDefault();
            if (c != null)
                return c;
            return new Country();
        }

        private void LoadRegions()
        {
            _Regions = new List<Region>();

            switch (this.IsoCode)
            {
                // United States
                case "US":
                    _Regions.Add(new Region("Alabama", "AL"));
                    _Regions.Add(new Region("Alaska", "AK"));
                    _Regions.Add(new Region("Arizona", "AZ"));
                    _Regions.Add(new Region("Arkansas", "AR"));
                    _Regions.Add(new Region("Armed Forces Africa", "AE"));
                    _Regions.Add(new Region("Armed Forces Americas", "AA"));
                    _Regions.Add(new Region("Armed Forces Canada", "AE"));
                    _Regions.Add(new Region("Armed Forces Europe", "AE"));
                    _Regions.Add(new Region("Armed Forces Middle", "AE"));
                    _Regions.Add(new Region("Armed Forces Pacific", "AP"));
                    _Regions.Add(new Region("California", "CA"));
                    _Regions.Add(new Region("Colorado", "CO"));
                    _Regions.Add(new Region("Connecticut", "CT"));
                    _Regions.Add(new Region("Delaware", "DE"));
                    _Regions.Add(new Region("District Of Columbia", "DC"));
                    _Regions.Add(new Region("Florida", "FL"));
                    _Regions.Add(new Region("Georgia", "GA"));
                    _Regions.Add(new Region("Hawaii", "HI"));
                    _Regions.Add(new Region("Idaho", "ID"));
                    _Regions.Add(new Region("Illinois", "IL"));
                    _Regions.Add(new Region("Indiana", "IN"));
                    _Regions.Add(new Region("Iowa", "IA"));
                    _Regions.Add(new Region("Kansas", "KS"));
                    _Regions.Add(new Region("Kentucky", "KY"));
                    _Regions.Add(new Region("Louisiana", "LA"));
                    _Regions.Add(new Region("Maine", "ME"));
                    _Regions.Add(new Region("Maryland", "MD"));
                    _Regions.Add(new Region("Massachusetts", "MA"));
                    _Regions.Add(new Region("Michigan", "MI"));
                    _Regions.Add(new Region("Minnesota", "MN"));
                    _Regions.Add(new Region("Mississippi", "MS"));
                    _Regions.Add(new Region("Missouri", "MO"));
                    _Regions.Add(new Region("Montana", "MT"));
                    _Regions.Add(new Region("Nebraska", "NE"));
                    _Regions.Add(new Region("Nevada", "NV"));
                    _Regions.Add(new Region("New Hampshire", "NH"));
                    _Regions.Add(new Region("New Jersey", "NJ"));
                    _Regions.Add(new Region("New Mexico", "NM"));
                    _Regions.Add(new Region("New York", "NY"));
                    _Regions.Add(new Region("North Carolina", "NC"));
                    _Regions.Add(new Region("North Dakota", "ND"));
                    _Regions.Add(new Region("Ohio", "OH"));
                    _Regions.Add(new Region("Oklahoma", "OK"));
                    _Regions.Add(new Region("Oregon", "OR"));
                    _Regions.Add(new Region("Pennsylvania", "PA"));
                    _Regions.Add(new Region("Rhode Island", "RI"));
                    _Regions.Add(new Region("South Carolina", "SC"));
                    _Regions.Add(new Region("South Dakota", "SD"));
                    _Regions.Add(new Region("Tennessee", "TN"));
                    _Regions.Add(new Region("Texas", "TX"));
                    _Regions.Add(new Region("U.S. Virgin Islands", "VI"));
                    _Regions.Add(new Region("Utah", "UT"));
                    _Regions.Add(new Region("Vermont", "VT"));
                    _Regions.Add(new Region("Virginia", "VA"));
                    _Regions.Add(new Region("Washington", "WA"));
                    _Regions.Add(new Region("West Virginia", "WV"));
                    _Regions.Add(new Region("Wisconsin", "WI"));
                    _Regions.Add(new Region("Wyoming", "WY"));
                    break;

                case "CA":
                    // CANADA
                    _Regions.Add(new Region("Alberta", "AB"));
                    _Regions.Add(new Region("British Columbia", "BC"));
                    _Regions.Add(new Region("Manitoba", "MB"));
                    _Regions.Add(new Region("New Brunswick", "NB"));
                    _Regions.Add(new Region("Newfoundland", "NL"));
                    _Regions.Add(new Region("Northwest Territories", "NT"));
                    _Regions.Add(new Region("Nova Scotia", "NS"));
                    _Regions.Add(new Region("Nunavut", "NU"));
                    _Regions.Add(new Region("Ontario", "ON"));
                    _Regions.Add(new Region("Prince Edward Island", "PE"));
                    _Regions.Add(new Region("Quebec", "QC"));
                    _Regions.Add(new Region("Saskatchewan", "SK"));
                    _Regions.Add(new Region("Yukon Territory", "YT"));
                    break;
                case "AU":
                    // Australia
                    _Regions.Add(new Region("Austrailian Capital Territory", "ACT"));
                    _Regions.Add(new Region("New South Wales", "NSW"));
                    _Regions.Add(new Region("Northern Territory", "NT"));
                    _Regions.Add(new Region("Queensland", "QLD"));
                    _Regions.Add(new Region("South Australia", "SA"));
                    _Regions.Add(new Region("Tasmania", "TAS"));
                    _Regions.Add(new Region("Victoria", "VIC"));
                    _Regions.Add(new Region("Western Australia", "WA"));
                    _Regions.Add(new Region("Cocos (Keeling) Islands", "CC"));
                    _Regions.Add(new Region("Christmas Island", "CX"));
                    _Regions.Add(new Region("Heard Island", "HM"));
                    _Regions.Add(new Region("Norfolk Island", "NF"));
                    break;
                case "CN":
                    // China
                    _Regions.Add(new Region("Beijing (北京)", "CN-11"));
                    _Regions.Add(new Region("Chongqing (重庆)", "CN-50"));
                    _Regions.Add(new Region("Shanghai (上海)", "CN-31"));
                    _Regions.Add(new Region("Tianjin (天津)", "CN-12"));
                    _Regions.Add(new Region("Anhui (安徽)", "CN-34"));
                    _Regions.Add(new Region("Fujian (福建)", "CN-35"));
                    _Regions.Add(new Region("Gansu (甘肃)", "CN-62"));
                    _Regions.Add(new Region("Guangdong (广东)", "CN-44"));
                    _Regions.Add(new Region("Guizhou (贵州)", "CN-52"));
                    _Regions.Add(new Region("Hainan (海南)", "CN-46"));
                    _Regions.Add(new Region("Hebei (河北)", "CN-13"));
                    _Regions.Add(new Region("Heilongjiang (黑龙江)", "CN-23"));
                    _Regions.Add(new Region("Henan (河南)", "CN-41"));
                    _Regions.Add(new Region("Hubei (湖北)", "CN-42"));
                    _Regions.Add(new Region("Hunan (湖南)", "CN-43"));
                    _Regions.Add(new Region("Jiangsu (江苏)", "CN-32"));
                    _Regions.Add(new Region("Jiangxi (江西)", "CN-36"));
                    _Regions.Add(new Region("Jilin (吉林)", "CN-22"));
                    _Regions.Add(new Region("Liaoning (吉林)", "CN-21"));
                    _Regions.Add(new Region("Qinghai (青海)", "CN-63"));
                    _Regions.Add(new Region("Shaanxi (陕西)", "CN-61"));
                    _Regions.Add(new Region("Shandong (山东)", "CN-37"));
                    _Regions.Add(new Region("Shanxi (山西)", "CN-14"));
                    _Regions.Add(new Region("Sichuan (四川)", "CN-51"));
                    _Regions.Add(new Region("Taiwan", "CN-71"));
                    _Regions.Add(new Region("Yunnan (云南)", "CN-53"));
                    _Regions.Add(new Region("Zhejiang (浙江)", "CN-33"));
                    _Regions.Add(new Region("Guangxi (广西壮族)", "CN-45"));
                    _Regions.Add(new Region("Nei Mongol (内蒙古)", "CN-15"));
                    _Regions.Add(new Region("Ningxia (宁夏回族)", "CN-64"));
                    _Regions.Add(new Region("Xinjiang (新疆维吾尔族)", "CN-65"));
                    _Regions.Add(new Region("Xizang", "CN-54"));
                    _Regions.Add(new Region("Hong Kong (香港)", "CN-91"));
                    _Regions.Add(new Region("Macau (澳门)", "CN-92"));
                    break;
                case "FR":
                    // France
                    _Regions.Add(new Region("Ain", "01"));
                    _Regions.Add(new Region("Aisne", "02"));
                    _Regions.Add(new Region("Allier", "03"));
                    _Regions.Add(new Region("Alpes-de-Haute-Provence", "04"));
                    _Regions.Add(new Region("Hautes-Alpes", "05"));
                    _Regions.Add(new Region("Alpes-Maritimes", "06"));
                    _Regions.Add(new Region("Ardèche", "07"));
                    _Regions.Add(new Region("Ardennes", "08"));
                    _Regions.Add(new Region("Ariège", "09"));
                    _Regions.Add(new Region("Aube", "10"));
                    _Regions.Add(new Region("Aude", "11"));
                    _Regions.Add(new Region("Aveyron", "12"));
                    _Regions.Add(new Region("Bouches-du-Rhône", "13"));
                    _Regions.Add(new Region("Calvados", "14"));
                    _Regions.Add(new Region("Cantal", "15"));
                    _Regions.Add(new Region("Charente", "16"));
                    _Regions.Add(new Region("Charente-Maritime", "17"));
                    _Regions.Add(new Region("Cher", "18"));
                    _Regions.Add(new Region("Corrèze", "19"));
                    _Regions.Add(new Region("Corse-du-Sud", "2A"));
                    _Regions.Add(new Region("Haute-Corse", "2B"));
                    _Regions.Add(new Region("Côte-d'Or", "21"));
                    _Regions.Add(new Region("Côtes-d'Armor", "22"));
                    _Regions.Add(new Region("Creuse", "23"));
                    _Regions.Add(new Region("Dordogne", "24"));
                    _Regions.Add(new Region("Doubs", "25"));
                    _Regions.Add(new Region("Drôme", "26"));
                    _Regions.Add(new Region("Eure", "27"));
                    _Regions.Add(new Region("Eure-et-Loir", "28"));
                    _Regions.Add(new Region("Finistère", "29"));
                    _Regions.Add(new Region("Gard", "30"));
                    _Regions.Add(new Region("Haute-Garonne", "31"));
                    _Regions.Add(new Region("Gers", "32"));
                    _Regions.Add(new Region("Gironde", "33"));
                    _Regions.Add(new Region("Hérault", "34"));
                    _Regions.Add(new Region("Ille-et-Vilaine", "35"));
                    _Regions.Add(new Region("Indre", "36"));
                    _Regions.Add(new Region("Indre-et-Loire", "37"));
                    _Regions.Add(new Region("Isère", "38"));
                    _Regions.Add(new Region("Jura", "39"));
                    _Regions.Add(new Region("Landes", "40"));
                    _Regions.Add(new Region("Loir-et-Cher", "41"));
                    _Regions.Add(new Region("Loire", "42"));
                    _Regions.Add(new Region("Haute-Loire", "43"));
                    _Regions.Add(new Region("Loire-Atlantique", "44"));
                    _Regions.Add(new Region("Loiret", "45"));
                    _Regions.Add(new Region("Lot", "46"));
                    _Regions.Add(new Region("Lot-et-Garonne", "47"));
                    _Regions.Add(new Region("Lozère", "48"));
                    _Regions.Add(new Region("Maine-et-Loire", "49"));
                    _Regions.Add(new Region("Manche", "50"));
                    _Regions.Add(new Region("Marne", "51"));
                    _Regions.Add(new Region("Haute-Marne", "52"));
                    _Regions.Add(new Region("Mayenne", "53"));
                    _Regions.Add(new Region("Meurthe-et-Moselle", "54"));
                    _Regions.Add(new Region("Meuse", "55"));
                    _Regions.Add(new Region("Morbihan", "56"));
                    _Regions.Add(new Region("Moselle", "57"));
                    _Regions.Add(new Region("Nièvre", "58"));
                    _Regions.Add(new Region("Nord", "59"));
                    _Regions.Add(new Region("Oise", "60"));
                    _Regions.Add(new Region("Orne", "61"));
                    _Regions.Add(new Region("Pas-de-Calais", "62"));
                    _Regions.Add(new Region("Puy-de-Dôme", "63"));
                    _Regions.Add(new Region("Pyrénées-Atlantiques", "64"));
                    _Regions.Add(new Region("Hautes-Pyrénées", "65"));
                    _Regions.Add(new Region("Pyrénées-Orientales", "66"));
                    _Regions.Add(new Region("Bas-Rhin", "67"));
                    _Regions.Add(new Region("Haut-Rhin", "68"));
                    _Regions.Add(new Region("Rhône", "69"));
                    _Regions.Add(new Region("Haute-Saône", "70"));
                    _Regions.Add(new Region("Saône-et-Loire", "71"));
                    _Regions.Add(new Region("Sarthe", "72"));
                    _Regions.Add(new Region("Savoie", "73"));
                    _Regions.Add(new Region("Haute-Savoie", "74"));
                    _Regions.Add(new Region("Paris", "75"));
                    _Regions.Add(new Region("Seine-Maritime", "76"));
                    _Regions.Add(new Region("Seine-et-Marne", "77"));
                    _Regions.Add(new Region("Yvelines", "78"));
                    _Regions.Add(new Region("Deux-Sèvres", "79"));
                    _Regions.Add(new Region("Somme", "80"));
                    _Regions.Add(new Region("Tarn", "81"));
                    _Regions.Add(new Region("Tarn-et-Garonne", "82"));
                    _Regions.Add(new Region("Var", "83"));
                    _Regions.Add(new Region("Vaucluse", "84"));
                    _Regions.Add(new Region("Vendée", "85"));
                    _Regions.Add(new Region("Vienne", "86"));
                    _Regions.Add(new Region("Haute-Vienne", "87"));
                    _Regions.Add(new Region("Vosges", "88"));
                    _Regions.Add(new Region("Yonne", "89"));
                    _Regions.Add(new Region("Territoire de Belfort", "90"));
                    _Regions.Add(new Region("Essonne", "91"));
                    _Regions.Add(new Region("Hauts-de-Seine", "92"));
                    _Regions.Add(new Region("Seine-Saint-Denis", "93"));
                    _Regions.Add(new Region("Val-de-Marne", "94"));
                    _Regions.Add(new Region("Val-d'Oise", "95"));
                    _Regions.Add(new Region("Clipperton Island", "CP"));
                    _Regions.Add(new Region("Saint Barthélemy", "BL"));
                    _Regions.Add(new Region("Saint Martin (French part)", "MF"));
                    _Regions.Add(new Region("New Caledonia", "NC"));
                    _Regions.Add(new Region("French Polynesia", "PF"));
                    _Regions.Add(new Region("Saint-Pierre and Miquelon", "PM"));
                    _Regions.Add(new Region("French Southern Territories", "TF"));
                    _Regions.Add(new Region("Wallis and Futuna", "WF"));
                    _Regions.Add(new Region("Mayotte", "YT"));
                    break;
                case "IL":
                    // Israel
                    _Regions.Add(new Region("Center District מחוז המרכז ", "M"));
                    _Regions.Add(new Region("Haifa District מחוז חיפה ", "HA"));
                    _Regions.Add(new Region("Jerusalem District מחוז ירושלים ", "JM"));
                    _Regions.Add(new Region("North District מחוז הצפון ", "Z"));
                    _Regions.Add(new Region("South District מחוז דרום ", "D"));
                    _Regions.Add(new Region("Tel Aviv", "TA"));
                    break;
                case "JP":
                    // Japan
                    _Regions.Add(new Region("Aiti (Aichi) (愛知)", "23"));
                    _Regions.Add(new Region("Akita (秋田)", "05"));
                    _Regions.Add(new Region("Aomori (青森)", "02"));
                    _Regions.Add(new Region("Ehime (愛媛)", "38"));
                    _Regions.Add(new Region("Gihu (Gifu) (岐阜)", "21"));
                    _Regions.Add(new Region("Gunma (群馬)", "10"));
                    _Regions.Add(new Region("Hirosima (Hiroshima) (広島)", "34"));
                    _Regions.Add(new Region("Hokkaidô (Hokkaidō) (北海道)", "01"));
                    _Regions.Add(new Region("Hukui (Fukui) (福井)", "18"));
                    _Regions.Add(new Region("Hukuoka (Fukuoka) (福岡)", "40"));
                    _Regions.Add(new Region("Hukusima (Fukushima) (福島)", "07"));
                    _Regions.Add(new Region("Hyôgo (Hyōgo) (兵庫)", "28"));
                    _Regions.Add(new Region("Ibaraki (茨城)", "08"));
                    _Regions.Add(new Region("Isikawa (Ishikawa) (石川)", "17"));
                    _Regions.Add(new Region("Iwate (岩手)", "03"));
                    _Regions.Add(new Region("Kagawa (香川)", "37"));
                    _Regions.Add(new Region("Kagosima (Kagoshima) (鹿児島)", "46"));
                    _Regions.Add(new Region("Kanagawa (神奈川)", "14"));
                    _Regions.Add(new Region("Kumamoto (熊本)", "43"));
                    _Regions.Add(new Region("Kyôto (Kyōto) (京都)", "26"));
                    _Regions.Add(new Region("Kôti (Kōchi) (高知)", "39"));
                    _Regions.Add(new Region("Mie (三重)", "24"));
                    _Regions.Add(new Region("Miyagi (宮城)", "04"));
                    _Regions.Add(new Region("Miyazaki (宮崎)", "45"));
                    _Regions.Add(new Region("Nagano (長野)", "20"));
                    _Regions.Add(new Region("Nagasaki (長崎)", "42"));
                    _Regions.Add(new Region("Nara (奈良)", "29"));
                    _Regions.Add(new Region("Niigata (新潟)", "15"));
                    _Regions.Add(new Region("Okayama (岡山)", "33"));
                    _Regions.Add(new Region("Okinawa (沖縄)", "47"));
                    _Regions.Add(new Region("Saga (佐賀)", "41"));
                    _Regions.Add(new Region("Saitama (埼玉)", "11"));
                    _Regions.Add(new Region("Siga (Shiga) (滋賀)", "25"));
                    _Regions.Add(new Region("Simane (Shimane) (島根)", "32"));
                    _Regions.Add(new Region("Sizuoka (Shizuoka) (静岡)", "22"));
                    _Regions.Add(new Region("Tiba (Chiba) (千葉)", "12"));
                    _Regions.Add(new Region("Tokusima (Tokushima) (徳島)", "36"));
                    _Regions.Add(new Region("Totigi (Tochigi) (栃木)", "09"));
                    _Regions.Add(new Region("Tottori (鳥取)", "31"));
                    _Regions.Add(new Region("Toyama (富山)", "16"));
                    _Regions.Add(new Region("Tôkyô (Tōkyō) (東京)", "13"));
                    _Regions.Add(new Region("Wakayama (和歌山)", "30"));
                    _Regions.Add(new Region("Yamagata (山形)", "06"));
                    _Regions.Add(new Region("Yamaguti (Yamaguchi) (山口)", "35"));
                    _Regions.Add(new Region("Yamanasi (Yamanashi) (山梨)", "19"));
                    _Regions.Add(new Region("Ôita (Ōita) (大分)", "44"));
                    _Regions.Add(new Region("Ôsaka (Ōsaka) (大阪)", "27"));
                    break;
                case "MX":
                    // Mexico
                    _Regions.Add(new Region("Aguascalientes", "AGU"));
                    _Regions.Add(new Region("Baja California", "BCN"));
                    _Regions.Add(new Region("Baja California Sur", "BCS"));
                    _Regions.Add(new Region("Campeche", "CAM"));
                    _Regions.Add(new Region("Chiapas", "CHP"));
                    _Regions.Add(new Region("Chihuahua", "CHH"));
                    _Regions.Add(new Region("Coahuila", "COA"));
                    _Regions.Add(new Region("Colima", "COL"));
                    _Regions.Add(new Region("Federal District", "DIF"));
                    _Regions.Add(new Region("Durango", "DUR"));
                    _Regions.Add(new Region("Guanajuato", "GUA"));
                    _Regions.Add(new Region("Guerrero", "GRO"));
                    _Regions.Add(new Region("Hidalgo", "HID"));
                    _Regions.Add(new Region("Jalisco", "JAL"));
                    _Regions.Add(new Region("Mexico State", "MEX"));
                    _Regions.Add(new Region("Michoacán", "MIC"));
                    _Regions.Add(new Region("Morelos", "MOR"));
                    _Regions.Add(new Region("Nayarit", "NAY"));
                    _Regions.Add(new Region("Nuevo León", "NLE"));
                    _Regions.Add(new Region("Oaxaca", "OAX"));
                    _Regions.Add(new Region("Puebla", "PUE"));
                    _Regions.Add(new Region("Querétaro", "QUE"));
                    _Regions.Add(new Region("Quintana Roo", "ROO"));
                    _Regions.Add(new Region("San Luis Potosí", "SLP"));
                    _Regions.Add(new Region("Sinaloa", "SIN"));
                    _Regions.Add(new Region("Sonora", "SON"));
                    _Regions.Add(new Region("Tabasco", "TAB"));
                    _Regions.Add(new Region("Tamaulipas", "TAM"));
                    _Regions.Add(new Region("Tlaxcala", "TLA"));
                    _Regions.Add(new Region("Veracruz", "VER"));
                    _Regions.Add(new Region("Yucatán", "YUC"));
                    _Regions.Add(new Region("Zacatecas", "ZAC"));
                    break;
                case "ES":
                    // Spain
                    _Regions.Add(new Region("Andalucía", "AN"));
                    _Regions.Add(new Region("Aragón", "AR"));
                    _Regions.Add(new Region("Asturias", "O"));
                    _Regions.Add(new Region("Balearic Islands", "IB"));
                    _Regions.Add(new Region("Basque Country", "PV"));
                    _Regions.Add(new Region("Canary Islands", "CN"));
                    _Regions.Add(new Region("Cantabria", "S"));
                    _Regions.Add(new Region("Castilla-La Mancha", "CM"));
                    _Regions.Add(new Region("Castilla y León", "CL"));
                    _Regions.Add(new Region("Catalonia", "CT"));
                    _Regions.Add(new Region("Extremadura", "EX"));
                    _Regions.Add(new Region("Galicia", "GA"));
                    _Regions.Add(new Region("La Rioja", "LO"));
                    _Regions.Add(new Region("Madrid", "M"));
                    _Regions.Add(new Region("Murcia", "MU"));
                    _Regions.Add(new Region("Navarre", "NA"));
                    _Regions.Add(new Region("Valencia", "VC"));
                    _Regions.Add(new Region("A Coruña", "C"));
                    _Regions.Add(new Region("Álava", "VI"));
                    _Regions.Add(new Region("Albacete", "AB"));
                    _Regions.Add(new Region("Alicante", "A"));
                    _Regions.Add(new Region("Almería", "AL"));
                    _Regions.Add(new Region("Asturias", "O"));
                    _Regions.Add(new Region("Ávila", "AV"));
                    _Regions.Add(new Region("Badajoz", "BA"));
                    _Regions.Add(new Region("Baleares", "PM"));
                    _Regions.Add(new Region("Barcelona", "B"));
                    _Regions.Add(new Region("Biscay", "BI"));
                    _Regions.Add(new Region("Burgos", "BU"));
                    _Regions.Add(new Region("Cáceres", "CC"));
                    _Regions.Add(new Region("Cádiz", "CA"));
                    _Regions.Add(new Region("Cantabria", "S"));
                    _Regions.Add(new Region("Castellón", "CS"));
                    _Regions.Add(new Region("Ciudad Real", "CR"));
                    _Regions.Add(new Region("Córdoba", "CO"));
                    _Regions.Add(new Region("Cuenca", "CU"));
                    _Regions.Add(new Region("Girona", "GI"));
                    _Regions.Add(new Region("Granada", "GR"));
                    _Regions.Add(new Region("Guadalajara", "GU"));
                    _Regions.Add(new Region("Guipúzcoa", "SS"));
                    _Regions.Add(new Region("Huelva", "H"));
                    _Regions.Add(new Region("Huesca", "HU"));
                    _Regions.Add(new Region("Jaén", "J"));
                    _Regions.Add(new Region("La Rioja", "LO"));
                    _Regions.Add(new Region("Las Palmas", "GC"));
                    _Regions.Add(new Region("León", "LE"));
                    _Regions.Add(new Region("Lleida", "L"));
                    _Regions.Add(new Region("Lugo", "LU"));
                    _Regions.Add(new Region("Madrid", "M"));
                    _Regions.Add(new Region("Málaga", "MA"));
                    _Regions.Add(new Region("Murcia", "MU"));
                    _Regions.Add(new Region("Navarre", "NA"));
                    _Regions.Add(new Region("Ourense", "OR"));
                    _Regions.Add(new Region("Palencia", "P"));
                    _Regions.Add(new Region("Pontevedra", "PO"));
                    _Regions.Add(new Region("Salamanca", "SA"));
                    _Regions.Add(new Region("Santa Cruz De Tenerife", "TF"));
                    _Regions.Add(new Region("Segovia", "SG"));
                    _Regions.Add(new Region("Seville", "SE"));
                    _Regions.Add(new Region("Soria", "SO"));
                    _Regions.Add(new Region("Tarragona", "T"));
                    _Regions.Add(new Region("Teruel", "TE"));
                    _Regions.Add(new Region("Toledo", "TO"));
                    _Regions.Add(new Region("Valencia", "V"));
                    _Regions.Add(new Region("Valladolid", "VA"));
                    _Regions.Add(new Region("Zamora", "ZA"));
                    _Regions.Add(new Region("Zaragoza", "Z"));
                    _Regions.Add(new Region("Ceuta", "CE"));
                    _Regions.Add(new Region("Melilla", "ML"));
                    break;
                case "GB":
                    // United Kingdom
                    _Regions.Add(new Region("Aberdeenshire", "ABD"));
                    _Regions.Add(new Region("Aberdeen", "ABE"));
                    _Regions.Add(new Region("Argyll and Bute", "AGB"));
                    _Regions.Add(new Region("Isle of Anglesey", "AGY"));
                    _Regions.Add(new Region("Angus", "ANS"));
                    _Regions.Add(new Region("Antrim", "ANT"));
                    _Regions.Add(new Region("Ards", "ARD"));
                    _Regions.Add(new Region("Armagh", "ARM"));
                    _Regions.Add(new Region("Bath and North East Somerset", "BAS"));
                    _Regions.Add(new Region("Blackburn with Darwen", "BBD"));
                    _Regions.Add(new Region("Bedfordshire", "BDF"));
                    _Regions.Add(new Region("Barking and Dagenham", "BDG"));
                    _Regions.Add(new Region("Brent", "BEN"));
                    _Regions.Add(new Region("Bexley", "BEX"));
                    _Regions.Add(new Region("Belfast", "BFS"));
                    _Regions.Add(new Region("Bridgend", "BGE"));
                    _Regions.Add(new Region("Blaenau Gwent", "BGW"));
                    _Regions.Add(new Region("Birmingham", "BIR"));
                    _Regions.Add(new Region("Buckinghamshire", "BKM"));
                    _Regions.Add(new Region("Ballymena", "BLA"));
                    _Regions.Add(new Region("Ballymoney", "BLY"));
                    _Regions.Add(new Region("Bournemouth", "BMH"));
                    _Regions.Add(new Region("Banbridge", "BNB"));
                    _Regions.Add(new Region("Barnet", "BNE"));
                    _Regions.Add(new Region("Brighton and Hove", "BNH"));
                    _Regions.Add(new Region("Barnsley", "BNS"));
                    _Regions.Add(new Region("Bolton", "BOL"));
                    _Regions.Add(new Region("Blackpool", "BPL"));
                    _Regions.Add(new Region("Bracknell Forest", "BRC"));
                    _Regions.Add(new Region("Bradford", "BRD"));
                    _Regions.Add(new Region("Bromley", "BRY"));
                    _Regions.Add(new Region("Bristol", "BST"));
                    _Regions.Add(new Region("Bury", "BUR"));
                    _Regions.Add(new Region("Cambridgeshire", "CAM"));
                    _Regions.Add(new Region("Caerphilly", "CAY"));
                    _Regions.Add(new Region("Ceredigion", "CGN"));
                    _Regions.Add(new Region("Craigavon", "CGV"));
                    _Regions.Add(new Region("Cheshire", "CHS"));
                    _Regions.Add(new Region("Carrickfergus", "CKF"));
                    _Regions.Add(new Region("Cookstown", "CKT"));
                    _Regions.Add(new Region("Calderdale", "CLD"));
                    _Regions.Add(new Region("Clackmannanshire", "CLK"));
                    _Regions.Add(new Region("Coleraine", "CLR"));
                    _Regions.Add(new Region("Cumbria", "CMA"));
                    _Regions.Add(new Region("Camden", "CMD"));
                    _Regions.Add(new Region("Carmarthenshire", "CMN"));
                    _Regions.Add(new Region("Cornwall", "CON"));
                    _Regions.Add(new Region("Coventry", "COV"));
                    _Regions.Add(new Region("Cardiff", "CRF"));
                    _Regions.Add(new Region("Croydon", "CRY"));
                    _Regions.Add(new Region("Castlereagh", "CSR"));
                    _Regions.Add(new Region("Conwy", "CWY"));
                    _Regions.Add(new Region("Darlington", "DAL"));
                    _Regions.Add(new Region("Derbyshire", "DBY"));
                    _Regions.Add(new Region("Denbighshire", "DEN"));
                    _Regions.Add(new Region("Derby", "DER"));
                    _Regions.Add(new Region("Devon", "DEV"));
                    _Regions.Add(new Region("Dungannon and South Tyrone", "DGN"));
                    _Regions.Add(new Region("Dumfries and Galloway", "DGY"));
                    _Regions.Add(new Region("Doncaster", "DNC"));
                    _Regions.Add(new Region("Dundee", "DND"));
                    _Regions.Add(new Region("Dorset", "DOR"));
                    _Regions.Add(new Region("Down", "DOW"));
                    _Regions.Add(new Region("Derry", "DRY"));
                    _Regions.Add(new Region("Dudley", "DUD"));
                    _Regions.Add(new Region("Durham", "DUR"));
                    _Regions.Add(new Region("Ealing", "EAL"));
                    _Regions.Add(new Region("East Ayrshire", "EAY"));
                    _Regions.Add(new Region("Edinburgh", "EDH"));
                    _Regions.Add(new Region("East Dunbartonshire", "EDU"));
                    _Regions.Add(new Region("East Lothian", "ELN"));
                    _Regions.Add(new Region("Eilean Siar", "ELS"));
                    _Regions.Add(new Region("Enfield", "ENF"));
                    _Regions.Add(new Region("East Renfrewshire", "ERW"));
                    _Regions.Add(new Region("East Riding of Yorkshire", "ERY"));
                    _Regions.Add(new Region("Essex", "ESS"));
                    _Regions.Add(new Region("East Sussex", "ESX"));
                    _Regions.Add(new Region("Falkirk", "FAL"));
                    _Regions.Add(new Region("Fermanagh", "FER"));
                    _Regions.Add(new Region("Fife", "FIF"));
                    _Regions.Add(new Region("Flintshire", "FLN"));
                    _Regions.Add(new Region("Gateshead", "GAT"));
                    _Regions.Add(new Region("Glasgow", "GLG"));
                    _Regions.Add(new Region("Gloucestershire", "GLS"));
                    _Regions.Add(new Region("Greenwich", "GRE"));
                    _Regions.Add(new Region("Guernsey", "GSY"));
                    _Regions.Add(new Region("Gwynedd", "GWN"));
                    _Regions.Add(new Region("Halton", "HAL"));
                    _Regions.Add(new Region("Hampshire", "HAM"));
                    _Regions.Add(new Region("Havering", "HAV"));
                    _Regions.Add(new Region("Hackney", "HCK "));
                    _Regions.Add(new Region("Herefordshire County", "HEF"));
                    _Regions.Add(new Region("Hillingdon", "HIL"));
                    _Regions.Add(new Region("Highland", "HLD"));
                    _Regions.Add(new Region("Hammersmith and Fulham", "HMF"));
                    _Regions.Add(new Region("Hounslow", "HNS"));
                    _Regions.Add(new Region("Hartlepool", "HPL"));
                    _Regions.Add(new Region("Hertfordshire", "HRT"));
                    _Regions.Add(new Region("Harrow", "HRW"));
                    _Regions.Add(new Region("Haringey", "HRY"));
                    _Regions.Add(new Region("Isles of Scilly", "IOS"));
                    _Regions.Add(new Region("Isle of Wight", "IOW"));
                    _Regions.Add(new Region("Islington", "ISL"));
                    _Regions.Add(new Region("Inverclyde", "IVC"));
                    _Regions.Add(new Region("Jersey", "JSY"));
                    _Regions.Add(new Region("Kensington and Chelsea", "KEC"));
                    _Regions.Add(new Region("Kent", "KEN"));
                    _Regions.Add(new Region("Kingston upon Hull", "KHL"));
                    _Regions.Add(new Region("Kirklees", "KIR"));
                    _Regions.Add(new Region("Kingston upon Thames", "KTT"));
                    _Regions.Add(new Region("Knowsley", "KWL"));
                    _Regions.Add(new Region("Lancashire", "LAN"));
                    _Regions.Add(new Region("Lambeth", "LBH"));
                    _Regions.Add(new Region("Leicester", "LCE"));
                    _Regions.Add(new Region("Leeds", "LDS"));
                    _Regions.Add(new Region("Leicestershire", "LEC"));
                    _Regions.Add(new Region("Lewisham", "LEW"));
                    _Regions.Add(new Region("Lincolnshire", "LIN"));
                    _Regions.Add(new Region("Liverpool", "LIV"));
                    _Regions.Add(new Region("Limavady", "LMV"));
                    _Regions.Add(new Region("London", "LND"));
                    _Regions.Add(new Region("Larne", "LRN"));
                    _Regions.Add(new Region("Lisburn", "LSB"));
                    _Regions.Add(new Region("Luton", "LUT"));
                    _Regions.Add(new Region("Manchester", "MAN"));
                    _Regions.Add(new Region("Middlesbrough", "MDB"));
                    _Regions.Add(new Region("Medway", "MDW"));
                    _Regions.Add(new Region("Magherafelt", "MFT"));
                    _Regions.Add(new Region("Milton Keynes", "MIK"));
                    _Regions.Add(new Region("Midlothian", "MLN"));
                    _Regions.Add(new Region("Monmouthshire", "MON"));
                    _Regions.Add(new Region("Merton", "MRT"));
                    _Regions.Add(new Region("Moray", "MRY"));
                    _Regions.Add(new Region("Merthyr Tydfil", "MTY"));
                    _Regions.Add(new Region("Moyle", "MYL"));
                    _Regions.Add(new Region("North Ayrshire", "NAY"));
                    _Regions.Add(new Region("Northumberland", "NBL"));
                    _Regions.Add(new Region("North Down", "NDN"));
                    _Regions.Add(new Region("North East Lincolnshire", "NEL"));
                    _Regions.Add(new Region("Newcastle upon Tyne", "NET"));
                    _Regions.Add(new Region("Norfolk", "NFK"));
                    _Regions.Add(new Region("Nottingham", "NGM"));
                    _Regions.Add(new Region("North Lanarkshire", "NLK"));
                    _Regions.Add(new Region("North Lincolnshire", "NLN"));
                    _Regions.Add(new Region("North Somerset", "NSM"));
                    _Regions.Add(new Region("Newtownabbey", "NTA"));
                    _Regions.Add(new Region("Northamptonshire", "NTH"));
                    _Regions.Add(new Region("Neath Port Talbot", "NTL"));
                    _Regions.Add(new Region("Nottinghamshire", "NTT"));
                    _Regions.Add(new Region("North Tyneside", "NTY"));
                    _Regions.Add(new Region("Newham", "NWM"));
                    _Regions.Add(new Region("Newport", "NWP"));
                    _Regions.Add(new Region("North Yorkshire", "NYK"));
                    _Regions.Add(new Region("Newry and Mourne", "NYM"));
                    _Regions.Add(new Region("Oldham", "OLD"));
                    _Regions.Add(new Region("Omagh", "OMH"));
                    _Regions.Add(new Region("Orkney Islands", "ORK"));
                    _Regions.Add(new Region("Oxfordshire", "OXF"));
                    _Regions.Add(new Region("Pembrokeshire", "PEM"));
                    _Regions.Add(new Region("Perth and Kinross", "PKN"));
                    _Regions.Add(new Region("Plymouth", "PLY"));
                    _Regions.Add(new Region("Poole", "POL"));
                    _Regions.Add(new Region("Portsmouth", "POR"));
                    _Regions.Add(new Region("Powys", "POW"));
                    _Regions.Add(new Region("Peterborough", "PTE"));
                    _Regions.Add(new Region("Redcar and Cleveland", "RCC"));
                    _Regions.Add(new Region("Rochdale", "RCH"));
                    _Regions.Add(new Region("Rhondda Cynon Taff", "RCT"));
                    _Regions.Add(new Region("Redbridge", "RDB"));
                    _Regions.Add(new Region("Reading", "RDG"));
                    _Regions.Add(new Region("Renfrewshire", "RFW"));
                    _Regions.Add(new Region("Richmond upon Thames", "RIC"));
                    _Regions.Add(new Region("Rotherham", "ROT"));
                    _Regions.Add(new Region("Rutland", "RUT"));
                    _Regions.Add(new Region("Sandwell", "SAW"));
                    _Regions.Add(new Region("South Ayrshire", "SAY"));
                    _Regions.Add(new Region("Scottish Borders, The", "SCB"));
                    _Regions.Add(new Region("Suffolk", "SFK"));
                    _Regions.Add(new Region("Sefton", "SFT"));
                    _Regions.Add(new Region("South Gloucestershire", "SGC"));
                    _Regions.Add(new Region("Sheffield", "SHF"));
                    _Regions.Add(new Region("St Helens", "SHN"));
                    _Regions.Add(new Region("Shropshire", "SHR"));
                    _Regions.Add(new Region("Stockport", "SKP"));
                    _Regions.Add(new Region("Salford", "SLF"));
                    _Regions.Add(new Region("Slough", "SLG"));
                    _Regions.Add(new Region("South Lanarkshire", "SLK"));
                    _Regions.Add(new Region("Sunderland", "SND"));
                    _Regions.Add(new Region("Solihull", "SOL"));
                    _Regions.Add(new Region("Somerset", "SOM"));
                    _Regions.Add(new Region("Southend-on-Sea", "SOS"));
                    _Regions.Add(new Region("Surrey", "SRY"));
                    _Regions.Add(new Region("Strabane", "STB"));
                    _Regions.Add(new Region("Stoke-on-Trent", "STE"));
                    _Regions.Add(new Region("Stirling", "STG"));
                    _Regions.Add(new Region("Southampton", "STH"));
                    _Regions.Add(new Region("Sutton", "STN"));
                    _Regions.Add(new Region("Staffordshire", "STS"));
                    _Regions.Add(new Region("Stockton-on-Tees", "STT"));
                    _Regions.Add(new Region("South Tyneside", "STY"));
                    _Regions.Add(new Region("Swansea", "SWA"));
                    _Regions.Add(new Region("Swindon", "SWD"));
                    _Regions.Add(new Region("Southwark", "SWK"));
                    _Regions.Add(new Region("Tameside", "TAM"));
                    _Regions.Add(new Region("Telford and Wrekin", "TFW"));
                    _Regions.Add(new Region("Thurrock", "THR"));
                    _Regions.Add(new Region("Torbay", "TOB"));
                    _Regions.Add(new Region("Torfaen", "TOF"));
                    _Regions.Add(new Region("Trafford", "TRF"));
                    _Regions.Add(new Region("Tower Hamlets", "TWH"));
                    _Regions.Add(new Region("Vale of Glamorgan", "VGL"));
                    _Regions.Add(new Region("Warwickshire", "WAR"));
                    _Regions.Add(new Region("West Berkshire", "WBK"));
                    _Regions.Add(new Region("West Dunbartonshire", "WDU"));
                    _Regions.Add(new Region("Waltham Forest", "WFT"));
                    _Regions.Add(new Region("Wigan", "WGN"));
                    _Regions.Add(new Region("Wiltshire", "WIL"));
                    _Regions.Add(new Region("Wakefield", "WKF"));
                    _Regions.Add(new Region("Walsall", "WLL"));
                    _Regions.Add(new Region("West Lothian", "WLN"));
                    _Regions.Add(new Region("Wolverhampton", "WLV"));
                    _Regions.Add(new Region("Wandsworth", "WND"));
                    _Regions.Add(new Region("Windsor and Maidenhead", "WNM"));
                    _Regions.Add(new Region("Wokingham", "WOK"));
                    _Regions.Add(new Region("Worcestershire", "WOR"));
                    _Regions.Add(new Region("Wirral", "WRL"));
                    _Regions.Add(new Region("Warrington", "WRT"));
                    _Regions.Add(new Region("Wrexham", "WRX"));
                    _Regions.Add(new Region("Westminster", "WSM"));
                    _Regions.Add(new Region("West Sussex", "WSX"));
                    _Regions.Add(new Region("York", "YOR"));
                    _Regions.Add(new Region("Shetland Islands", "ZET"));
                    break;
                case "AL":
                    _Regions.Add(new Region("Berat", "BR"));
                    _Regions.Add(new Region("Diber", "DI"));
                    _Regions.Add(new Region("Durres", "DR"));
                    _Regions.Add(new Region("Elbasan", "EL"));
                    _Regions.Add(new Region("Fier", "FR"));
                    _Regions.Add(new Region("Gjirokaster", "GJ"));
                    _Regions.Add(new Region("Gramsh", "GR"));
                    _Regions.Add(new Region("Kolonje", "ER"));
                    _Regions.Add(new Region("Korce", "KO"));
                    _Regions.Add(new Region("Kruje", "KR"));
                    _Regions.Add(new Region("Kukes", "KU"));
                    _Regions.Add(new Region("Lezhe", "LE"));
                    _Regions.Add(new Region("Librazhd", "LB"));
                    _Regions.Add(new Region("Lushnje", "LU"));
                    _Regions.Add(new Region("Mat", "MT"));
                    _Regions.Add(new Region("Mirdite", "MR"));
                    _Regions.Add(new Region("Permet", "PR"));
                    _Regions.Add(new Region("Pogradec", "PG"));
                    _Regions.Add(new Region("Puke", "PU"));
                    _Regions.Add(new Region("Sarande", "SR"));
                    _Regions.Add(new Region("Shkoder", "SH"));
                    _Regions.Add(new Region("Skrapar", "SK"));
                    _Regions.Add(new Region("Tepelene", "TE"));
                    _Regions.Add(new Region("Tropoje", "TP"));
                    _Regions.Add(new Region("Vlore", "VL"));
                    _Regions.Add(new Region("Tiran", "TI"));
                    _Regions.Add(new Region("Bulqize", "BU"));
                    _Regions.Add(new Region("Delvine", "DL"));
                    _Regions.Add(new Region("Devoll", "DV"));
                    _Regions.Add(new Region("Has", "HA"));
                    _Regions.Add(new Region("Kavaje", "KA"));
                    _Regions.Add(new Region("Kucove", "KC"));
                    _Regions.Add(new Region("Kurbin", "KB"));
                    _Regions.Add(new Region("Malesi e Madhe", "MM"));
                    _Regions.Add(new Region("Mallakaster", "MK"));
                    _Regions.Add(new Region("Peqin", "PQ"));
                    _Regions.Add(new Region("Tirane", "TR"));

                    break;
                case "DZ":
                    _Regions.Add(new Region("Alger", "AL"));
                    _Regions.Add(new Region("Batna", "BT"));
                    _Regions.Add(new Region("Constantine", "CO"));
                    _Regions.Add(new Region("Medea", "MD"));
                    _Regions.Add(new Region("Mostaganem", "MG"));
                    _Regions.Add(new Region("Oran", "OR"));
                    _Regions.Add(new Region("Saida", "SD"));
                    _Regions.Add(new Region("Setif", "SF"));
                    _Regions.Add(new Region("Tiaret", "TR"));
                    _Regions.Add(new Region("Tizi Ouzou", "TO"));
                    _Regions.Add(new Region("Tlemcen", "TL"));
                    _Regions.Add(new Region("Bejaia", "BJ"));
                    _Regions.Add(new Region("Biskra", "BS"));
                    _Regions.Add(new Region("Blida", "BL"));
                    _Regions.Add(new Region("Bouira", "BU"));
                    _Regions.Add(new Region("Djelfa", "DJ"));
                    _Regions.Add(new Region("Guelma", "GL"));
                    _Regions.Add(new Region("Jijel", "JJ"));
                    _Regions.Add(new Region("Laghouat", "LG"));
                    _Regions.Add(new Region("Mascara", "MC"));
                    _Regions.Add(new Region("M'Sila", "MS"));
                    _Regions.Add(new Region("Oum el Bouaghi", "OB"));
                    _Regions.Add(new Region("Sidi Bel Abbes", "SB"));
                    _Regions.Add(new Region("Skikda", "SK"));
                    _Regions.Add(new Region("Tebessa", "TB"));
                    _Regions.Add(new Region("Adrar", "AR"));
                    _Regions.Add(new Region("Ain Defla", "AD"));
                    _Regions.Add(new Region("Ain Temouchent", "AT"));
                    _Regions.Add(new Region("Annaba", "AN"));
                    _Regions.Add(new Region("Bechar", "BC"));
                    _Regions.Add(new Region("Bordj Bou Arreridj", "BB"));
                    _Regions.Add(new Region("Boumerdes", "BM"));
                    _Regions.Add(new Region("Chlef", "CH"));
                    _Regions.Add(new Region("El Bayadh", "EB"));
                    _Regions.Add(new Region("El Oued", "EO"));
                    _Regions.Add(new Region("El Tarf", "ET"));
                    _Regions.Add(new Region("Ghardaia", "GR"));
                    _Regions.Add(new Region("Illizi", "IL"));
                    _Regions.Add(new Region("Khenchela", "KH"));
                    _Regions.Add(new Region("Mila", "ML"));
                    _Regions.Add(new Region("Naama", "NA"));
                    _Regions.Add(new Region("Ouargla", "OG"));
                    _Regions.Add(new Region("Relizane", "RE"));
                    _Regions.Add(new Region("Souk Ahras", "SA"));
                    _Regions.Add(new Region("Tamanghasset", "TM"));
                    _Regions.Add(new Region("Tindouf", "TN"));
                    _Regions.Add(new Region("Tipaza", "TP"));
                    _Regions.Add(new Region("Tissemsilt", "TS"));

                    break;
                case "AR": // Argentia
                    _Regions.Add(new Region("Buenos Aires", "BA"));
                    _Regions.Add(new Region("Catamarca", "CT"));
                    _Regions.Add(new Region("Chaco", "CC"));
                    _Regions.Add(new Region("Chubut", "CH"));
                    _Regions.Add(new Region("Cordoba", "CB"));
                    _Regions.Add(new Region("Corrientes", "CN"));
                    _Regions.Add(new Region("Distrito Federal", "DF"));
                    _Regions.Add(new Region("Entre Rios", "ER"));
                    _Regions.Add(new Region("Formosa", "FM"));
                    _Regions.Add(new Region("Jujuy", "JY"));
                    _Regions.Add(new Region("La Pampa", "LP"));
                    _Regions.Add(new Region("La Rioja", "LR"));
                    _Regions.Add(new Region("Mendoza", "MZ"));
                    _Regions.Add(new Region("Misiones", "MN"));
                    _Regions.Add(new Region("Neuquen", "NQ"));
                    _Regions.Add(new Region("Rio Negro", "RN"));
                    _Regions.Add(new Region("Salta", "SA"));
                    _Regions.Add(new Region("San Juan", "SJ"));
                    _Regions.Add(new Region("San Luis", "SL"));
                    _Regions.Add(new Region("Santa Cruz", "SC"));
                    _Regions.Add(new Region("Santa Fe", "SF"));
                    _Regions.Add(new Region("Santiago del Estero", "SE"));
                    _Regions.Add(new Region("Tierra del Fuego", "TF"));
                    _Regions.Add(new Region("Tucuman", "TM"));
                    break;
                case "AM": // Armenia
                    _Regions.Add(new Region("Aragatsotn", "AG"));
                    _Regions.Add(new Region("Ararat", "AR"));
                    _Regions.Add(new Region("Armavir", "AV"));
                    _Regions.Add(new Region("Geghark'unik'", "GR"));
                    _Regions.Add(new Region("Kotayk'", "KT"));
                    _Regions.Add(new Region("Lorri", "LO"));
                    _Regions.Add(new Region("Shirak", "SH"));
                    _Regions.Add(new Region("Syunik'", "SU"));
                    _Regions.Add(new Region("Tavush", "TV"));
                    _Regions.Add(new Region("Vayots' Dzor", "VD"));
                    _Regions.Add(new Region("Yerevan", "ER"));
                    break;
                case "AT": // Austria
                    _Regions.Add(new Region("Burgenland", "BU"));
                    _Regions.Add(new Region("Karnten", "KA"));
                    _Regions.Add(new Region("Niederosterreich", "NO"));
                    _Regions.Add(new Region("Oberosterreich", "OO"));
                    _Regions.Add(new Region("Salzburg", "SZ"));
                    _Regions.Add(new Region("Steiermark", "ST"));
                    _Regions.Add(new Region("Tirol", "TR"));
                    _Regions.Add(new Region("Vorarlberg", "VO"));
                    _Regions.Add(new Region("Wien", "WI"));
                    break;
                case "BR": // Brazil
                    _Regions.Add(new Region("Acre", "AC"));
                    _Regions.Add(new Region("Amapá", "AP"));
                    _Regions.Add(new Region("Bahia", "BA"));
                    _Regions.Add(new Region("Goiás", "GO"));
                    _Regions.Add(new Region("Piauí", "PI"));
                    _Regions.Add(new Region("Ceará", "CE"));
                    _Regions.Add(new Region("Paraná", "PR"));
                    _Regions.Add(new Region("Alagoas", "AL"));
                    _Regions.Add(new Region("Paraíba", "PB"));
                    _Regions.Add(new Region("Roraima", "RR"));
                    _Regions.Add(new Region("Sergipe", "SE"));
                    _Regions.Add(new Region("Amazonas", "AM"));
                    _Regions.Add(new Region("Maranhão", "MA"));
                    _Regions.Add(new Region("Rondônia", "RO"));
                    _Regions.Add(new Region("São Paulo", "SP"));
                    _Regions.Add(new Region("Tocantins", "TO"));
                    _Regions.Add(new Region("Mato Grosso", "MT"));
                    _Regions.Add(new Region("Minas Gerais", "MG"));
                    _Regions.Add(new Region("Espírito Santo", "ES"));
                    _Regions.Add(new Region("Rio de Janeiro", "RJ"));
                    _Regions.Add(new Region("Santa Catarina", "SC"));
                    _Regions.Add(new Region("Rio Grande do Sul", "RS"));
                    _Regions.Add(new Region("Mato Grosso do Sul", "MS"));
                    _Regions.Add(new Region("Rio Grande do Norte", "RN"));
                    _Regions.Add(new Region("Distrito Federal", "DF"));
                    _Regions.Add(new Region("Paro", "PA"));
                    _Regions.Add(new Region("Pernambuco", "PE"));
                    break;
                default:
                    _Regions.Add(new Region("- Not Required -", ""));
                    break;
            }
        }
    }


}
