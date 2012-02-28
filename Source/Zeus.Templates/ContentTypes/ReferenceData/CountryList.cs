using Ext.Net;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes.ReferenceData
{
	[ContentType("Country List", Description = "Container for countries")]
	[RestrictParents(typeof(ReferenceDataNode))]
	public class CountryList : BaseContentItem
	{
		public CountryList()
		{
			Name = "countries";
			Title = "Countries";
		}

		protected override Icon Icon
		{
			get { return Icon.World; }
		}

		protected override void OnAfterCreate()
		{
			Children.Create(new Country("AF", "AFGHANISTAN", "Afghanistan", "AFG", "004"));
			Children.Create(new Country("AX", "ALAND ISLANDS", "Aland Islands", "ALA", "000"));
			Children.Create(new Country("AL", "ALBANIA", "Albania", "ALB", "008"));
			Children.Create(new Country("DZ", "ALGERIA", "Algeria", "DZA", "012"));
			Children.Create(new Country("AS", "AMERICAN SAMOA", "American Samoa", "ASM", "016"));
			Children.Create(new Country("AD", "ANDORRA", "Andorra", "AND", "020"));
			Children.Create(new Country("AO", "ANGOLA", "Angola", "AGO", "024"));
			Children.Create(new Country("AI", "ANGUILLA", "Anguilla", "AIA", "660"));
			Children.Create(new Country("AQ", "ANTARCTICA", "Antarctica", "ATA", "010"));
			Children.Create(new Country("AG", "ANTIGUA AND BARBUDA", "Antigua and Barbuda", "ATG", "028"));
			Children.Create(new Country("AR", "ARGENTINA", "Argentina", "ARG", "032"));
			Children.Create(new Country("AM", "ARMENIA", "Armenia", "ARM", "051"));
			Children.Create(new Country("AW", "ARUBA", "Aruba", "ABW", "533"));
			Children.Create(new Country("AU", "AUSTRALIA", "Australia", "AUS", "036"));
			Children.Create(new Country("AT", "AUSTRIA", "Austria", "AUT", "040"));
			Children.Create(new Country("AZ", "AZERBAIJAN", "Azerbaijan", "AZE", "031"));

			Children.Create(new Country("BS", "BAHAMAS", "Bahamas", "BHS", "044"));
			Children.Create(new Country("BH", "BAHRAIN", "Bahrain", "BHR", "048"));
			Children.Create(new Country("BD", "BANGLADESH", "Bangladesh", "BGD", "050"));
			Children.Create(new Country("BB", "BARBADOS", "Barbados", "BRB", "052"));
			Children.Create(new Country("BY", "BELARUS", "Belarus", "BLR", "112"));
			Children.Create(new Country("BE", "BELGIUM", "Belgium", "BEL", "056"));
			Children.Create(new Country("BZ", "BELIZE", "Belize", "BLZ", "084"));
			Children.Create(new Country("BJ", "BENIN", "Benin", "BEN", "204"));
			Children.Create(new Country("BM", "BERMUDA", "Bermuda", "BMU", "060"));
			Children.Create(new Country("BT", "BHUTAN", "Bhutan", "BTN", "064"));
			Children.Create(new Country("BO", "BOLIVIA", "Bolivia", "BOL", "068"));
			Children.Create(new Country("BQ", "BONAIRE, SAINT EUSTATIUS AND SABA", "Bonaire, Saint Eustatius And Saba", "BES", "000"));
			Children.Create(new Country("BA", "BOSNIA AND HERZEGOVINA", "Bosnia and Herzegovina", "BIH", "070"));
			Children.Create(new Country("BW", "BOTSWANA", "Botswana", "BWA", "072"));
			Children.Create(new Country("BV", "BOUVET ISLAND", "Bouvet Island", "BVT", "074"));
			Children.Create(new Country("BR", "BRAZIL", "Brazil", "BRA", "076"));
			Children.Create(new Country("IO", "BRITISH INDIAN OCEAN TERRITORY", "British Indian Ocean Territory", "IOT", "086"));
			Children.Create(new Country("BN", "BRUNEI DARUSSALAM", "Brunei Darussalam", "BRN", "096"));
			Children.Create(new Country("BG", "BULGARIA", "Bulgaria", "BGR", "100"));
			Children.Create(new Country("BF", "BURKINA FASO", "Burkina Faso", "BFA", "854"));
			Children.Create(new Country("BI", "BURUNDI", "Burundi", "BDI", "108"));

			Children.Create(new Country("KH", "CAMBODIA", "Cambodia", "KHM", "116"));
			Children.Create(new Country("CM", "CAMEROON", "Cameroon", "CMR", "120"));
			Children.Create(new Country("CA", "CANADA", "Canada", "CAN", "124"));
			Children.Create(new Country("CV", "CAPE VERDE", "Cape Verde", "CPV", "132"));
			Children.Create(new Country("KY", "CAYMAN ISLANDS", "Cayman Islands", "CYM", "136"));
			Children.Create(new Country("CF", "CENTRAL AFRICAN REPUBLIC", "Central African Republic", "CAF", "140"));
			Children.Create(new Country("TD", "CHAD", "Chad", "TCD", "148"));
			Children.Create(new Country("CL", "CHILE", "Chile", "CHL", "152"));
			Children.Create(new Country("CN", "CHINA", "China", "CHN", "156"));
			Children.Create(new Country("CX", "CHRISTMAS ISLAND", "Christmas Island", "CXR", "162"));
			Children.Create(new Country("CC", "COCOS (KEELING)) ISLANDS", "Cocos (Keeling)) Islands", "CCK", "166"));
			Children.Create(new Country("CO", "COLOMBIA", "Colombia", "COL", "170"));
			Children.Create(new Country("KM", "COMOROS", "Comoros", "COM", "174"));
			Children.Create(new Country("CG", "CONGO", "Congo", "COG", "178"));
			Children.Create(new Country("CD", "CONGO, THE DEMOCRATIC REPUBLIC OF THE", "Congo, the Democratic Republic of the", "COD", "180"));
			Children.Create(new Country("CK", "COOK ISLANDS", "Cook Islands", "COK", "184"));
			Children.Create(new Country("CR", "COSTA RICA", "Costa Rica", "CRI", "188"));
			Children.Create(new Country("CI", "COTE D'IVOIRE", "Cote D'Ivoire", "CIV", "384"));
			Children.Create(new Country("HR", "CROATIA", "Croatia", "HRV", "191"));
			Children.Create(new Country("CU", "CUBA", "Cuba", "CUB", "192"));
			Children.Create(new Country("CW", "CURACAO", "Curacao", "CUW", "000"));
			Children.Create(new Country("CY", "CYPRUS", "Cyprus", "CYP", "196"));
			Children.Create(new Country("CZ", "CZECH REPUBLIC", "Czech Republic", "CZE", "203"));

			Children.Create(new Country("DK", "DENMARK", "Denmark", "DNK", "208"));
			Children.Create(new Country("DJ", "DJIBOUTI", "Djibouti", "DJI", "262"));
			Children.Create(new Country("DM", "DOMINICA", "Dominica", "DMA", "212"));
			Children.Create(new Country("DO", "DOMINICAN REPUBLIC", "Dominican Republic", "DOM", "214"));

			Children.Create(new Country("EC", "ECUADOR", "Ecuador", "ECU", "218"));
			Children.Create(new Country("EG", "EGYPT", "Egypt", "EGY", "818"));
			Children.Create(new Country("SV", "EL SALVADOR", "El Salvador", "SLV", "222"));
			Children.Create(new Country("GQ", "EQUATORIAL GUINEA", "Equatorial Guinea", "GNQ", "226"));
			Children.Create(new Country("ER", "ERITREA", "Eritrea", "ERI", "232"));
			Children.Create(new Country("EE", "ESTONIA", "Estonia", "EST", "233"));
			Children.Create(new Country("ET", "ETHIOPIA", "Ethiopia", "ETH", "231"));

			Children.Create(new Country("FK", "FALKLAND ISLANDS (MALVINAS))", "Falkland Islands (Malvinas))", "FLK", "238"));
			Children.Create(new Country("FO", "FAROE ISLANDS", "Faroe Islands", "FRO", "234"));
			Children.Create(new Country("FJ", "FIJI", "Fiji", "FJI", "242"));
			Children.Create(new Country("FI", "FINLAND", "Finland", "FIN", "246"));
			Children.Create(new Country("FR", "FRANCE", "France", "FRA", "250"));
			Children.Create(new Country("GF", "FRENCH GUIANA", "French Guiana", "GUF", "254"));
			Children.Create(new Country("PF", "FRENCH POLYNESIA", "French Polynesia", "PYF", "258"));
			Children.Create(new Country("TF", "FRENCH SOUTHERN TERRITORIES", "French Southern Territories", "ATF", "260"));

			Children.Create(new Country("GA", "GABON", "Gabon", "GAB", "266"));
			Children.Create(new Country("GM", "GAMBIA", "Gambia", "GMB", "270"));
			Children.Create(new Country("GE", "GEORGIA", "Georgia", "GEO", "268"));
			Children.Create(new Country("DE", "GERMANY", "Germany", "DEU", "276"));
			Children.Create(new Country("GH", "GHANA", "Ghana", "GHA", "288"));
			Children.Create(new Country("GI", "GIBRALTAR", "Gibraltar", "GIB", "292"));
			Children.Create(new Country("GR", "GREECE", "Greece", "GRC", "300"));
			Children.Create(new Country("GL", "GREENLAND", "Greenland", "GRL", "304"));
			Children.Create(new Country("GD", "GRENADA", "Grenada", "GRD", "308"));
			Children.Create(new Country("GP", "GUADELOUPE", "Guadeloupe", "GLP", "312"));
			Children.Create(new Country("GU", "GUAM", "Guam", "GUM", "316"));
			Children.Create(new Country("GT", "GUATEMALA", "Guatemala", "GTM", "320"));
			Children.Create(new Country("GG", "GUERNSEY", "Guernsey", "GGY", "000"));
			Children.Create(new Country("GN", "GUINEA", "Guinea", "GIN", "324"));
			Children.Create(new Country("GW", "GUINEA-BISSAU", "Guinea-Bissau", "GNB", "624"));
			Children.Create(new Country("GY", "GUYANA", "Guyana", "GUY", "328"));

			Children.Create(new Country("HT", "HAITI", "Haiti", "HTI", "332"));
			Children.Create(new Country("HM", "HEARD ISLAND AND MCDONALD ISLANDS", "Heard Island and Mcdonald Islands", "HMD", "334"));
			Children.Create(new Country("VA", "HOLY SEE (VATICAN CITY STATE))", "Holy See (Vatican City State))", "VAT", "336"));
			Children.Create(new Country("HN", "HONDURAS", "Honduras", "HND", "340"));
			Children.Create(new Country("HK", "HONG KONG", "Hong Kong", "HKG", "344"));
			Children.Create(new Country("HU", "HUNGARY", "Hungary", "HUN", "348"));

			Children.Create(new Country("IS", "ICELAND", "Iceland", "ISL", "352"));
			Children.Create(new Country("IN", "INDIA", "India", "IND", "356"));
			Children.Create(new Country("ID", "INDONESIA", "Indonesia", "IDN", "360"));
			Children.Create(new Country("IR", "IRAN, ISLAMIC REPUBLIC OF", "Iran, Islamic Republic of", "IRN", "364"));
			Children.Create(new Country("IQ", "IRAQ", "Iraq", "IRQ", "368"));
			Children.Create(new Country("IE", "IRELAND", "Ireland", "IRL", "372"));
			Children.Create(new Country("IM", "ISLE OF MAN", "Isle Of Man", "IMN", "000"));
			Children.Create(new Country("IL", "ISRAEL", "Israel", "ISR", "376"));
			Children.Create(new Country("IT", "ITALY", "Italy", "ITA", "380"));

			Children.Create(new Country("JM", "JAMAICA", "Jamaica", "JAM", "388"));
			Children.Create(new Country("JP", "JAPAN", "Japan", "JPN", "392"));
			Children.Create(new Country("JE", "JERSEY", "Jersey", "JEY", "000"));
			Children.Create(new Country("JO", "JORDAN", "Jordan", "JOR", "400"));

			Children.Create(new Country("KZ", "KAZAKHSTAN", "Kazakhstan", "KAZ", "398"));
			Children.Create(new Country("KE", "KENYA", "Kenya", "KEN", "404"));
			Children.Create(new Country("KI", "KIRIBATI", "Kiribati", "KIR", "296"));
			Children.Create(new Country("KP", "KOREA, DEMOCRATIC PEOPLE'S REPUBLIC OF", "Korea, Democratic People's Republic of", "PRK", "408"));
			Children.Create(new Country("KR", "KOREA, REPUBLIC OF", "Korea, Republic of", "KOR", "410"));
			Children.Create(new Country("KW", "KUWAIT", "Kuwait", "KWT", "414"));
			Children.Create(new Country("KG", "KYRGYZSTAN", "Kyrgyzstan", "KGZ", "417"));

			Children.Create(new Country("LA", "LAO PEOPLE'S DEMOCRATIC REPUBLIC", "Lao People's Democratic Republic", "LAO", "418"));
			Children.Create(new Country("LV", "LATVIA", "Latvia", "LVA", "428"));
			Children.Create(new Country("LB", "LEBANON", "Lebanon", "LBN", "422"));
			Children.Create(new Country("LS", "LESOTHO", "Lesotho", "LSO", "426"));
			Children.Create(new Country("LR", "LIBERIA", "Liberia", "LBR", "430"));
			Children.Create(new Country("LY", "LIBYAN ARAB JAMAHIRIYA", "Libyan Arab Jamahiriya", "LBY", "434"));
			Children.Create(new Country("LI", "LIECHTENSTEIN", "Liechtenstein", "LIE", "438"));
			Children.Create(new Country("LT", "LITHUANIA", "Lithuania", "LTU", "440"));
			Children.Create(new Country("LU", "LUXEMBOURG", "Luxembourg", "LUX", "442"));

			Children.Create(new Country("MO", "MACAO", "Macao", "MAC", "446"));
			Children.Create(new Country("MK", "MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF", "Macedonia, the Former Yugoslav Republic of", "MKD", "807"));
			Children.Create(new Country("MG", "MADAGASCAR", "Madagascar", "MDG", "450"));
			Children.Create(new Country("MW", "MALAWI", "Malawi", "MWI", "454"));
			Children.Create(new Country("MY", "MALAYSIA", "Malaysia", "MYS", "458"));
			Children.Create(new Country("MV", "MALDIVES", "Maldives", "MDV", "462"));
			Children.Create(new Country("ML", "MALI", "Mali", "MLI", "466"));
			Children.Create(new Country("MT", "MALTA", "Malta", "MLT", "470"));
			Children.Create(new Country("MH", "MARSHALL ISLANDS", "Marshall Islands", "MHL", "584"));
			Children.Create(new Country("MQ", "MARTINIQUE", "Martinique", "MTQ", "474"));
			Children.Create(new Country("MR", "MAURITANIA", "Mauritania", "MRT", "478"));
			Children.Create(new Country("MU", "MAURITIUS", "Mauritius", "MUS", "480"));
			Children.Create(new Country("YT", "MAYOTTE", "Mayotte", "MYT", "175"));
			Children.Create(new Country("MX", "MEXICO", "Mexico", "MEX", "484"));
			Children.Create(new Country("FM", "MICRONESIA, FEDERATED STATES OF", "Micronesia, Federated States of", "FSM", "583"));
			Children.Create(new Country("MD", "MOLDOVA, REPUBLIC OF", "Moldova, Republic of", "MDA", "498"));
			Children.Create(new Country("MC", "MONACO", "Monaco", "MCO", "492"));
			Children.Create(new Country("MN", "MONGOLIA", "Mongolia", "MNG", "496"));
			Children.Create(new Country("ME", "MONTENEGRO", "Montenegro", "MNE", "499"));
			Children.Create(new Country("MS", "MONTSERRAT", "Montserrat", "MSR", "500"));
			Children.Create(new Country("MA", "MOROCCO", "Morocco", "MAR", "504"));
			Children.Create(new Country("MZ", "MOZAMBIQUE", "Mozambique", "MOZ", "508"));
			Children.Create(new Country("MM", "MYANMAR", "Myanmar", "MMR", "104"));

			Children.Create(new Country("NA", "NAMIBIA", "Namibia", "NAM", "516"));
			Children.Create(new Country("NR", "NAURU", "Nauru", "NRU", "520"));
			Children.Create(new Country("NP", "NEPAL", "Nepal", "NPL", "524"));
			Children.Create(new Country("NL", "NETHERLANDS", "Netherlands", "NLD", "528"));
			Children.Create(new Country("NC", "NEW CALEDONIA", "New Caledonia", "NCL", "540"));
			Children.Create(new Country("NZ", "NEW ZEALAND", "New Zealand", "NZL", "554"));
			Children.Create(new Country("NI", "NICARAGUA", "Nicaragua", "NIC", "558"));
			Children.Create(new Country("NE", "NIGER", "Niger", "NER", "562"));
			Children.Create(new Country("NG", "NIGERIA", "Nigeria", "NGA", "566"));
			Children.Create(new Country("NU", "NIUE", "Niue", "NIU", "570"));
			Children.Create(new Country("NF", "NORFOLK ISLAND", "Norfolk Island", "NFK", "574"));
			Children.Create(new Country("MP", "NORTHERN MARIANA ISLANDS", "Northern Mariana Islands", "MNP", "580"));
			Children.Create(new Country("NO", "NORWAY", "Norway", "NOR", "578"));

			Children.Create(new Country("OM", "OMAN", "Oman", "OMN", "512"));

			Children.Create(new Country("PK", "PAKISTAN", "Pakistan", "PAK", "586"));
			Children.Create(new Country("PW", "PALAU", "Palau", "PLW", "585"));
			Children.Create(new Country("PS", "PALESTINIAN TERRITORY, OCCUPIED", "Palestinian Territory, Occupied", "PSE", "275"));
			Children.Create(new Country("PA", "PANAMA", "Panama", "PAN", "591"));
			Children.Create(new Country("PG", "PAPUA NEW GUINEA", "Papua New Guinea", "PNG", "598"));
			Children.Create(new Country("PY", "PARAGUAY", "Paraguay", "PRY", "600"));
			Children.Create(new Country("PE", "PERU", "Peru", "PER", "604"));
			Children.Create(new Country("PH", "PHILIPPINES", "Philippines", "PHL", "608"));
			Children.Create(new Country("PN", "PITCAIRN", "Pitcairn", "PCN", "612"));
			Children.Create(new Country("PL", "POLAND", "Poland", "POL", "616"));
			Children.Create(new Country("PT", "PORTUGAL", "Portugal", "PRT", "620"));
			Children.Create(new Country("PR", "PUERTO RICO", "Puerto Rico", "PRI", "630"));

			Children.Create(new Country("QA", "QATAR", "Qatar", "QAT", "634"));

			Children.Create(new Country("RE", "REUNION", "Reunion", "REU", "638"));
			Children.Create(new Country("RO", "ROMANIA", "Romania", "ROM", "642"));
			Children.Create(new Country("RU", "RUSSIAN FEDERATION", "Russian Federation", "RUS", "643"));
			Children.Create(new Country("RW", "RWANDA", "Rwanda", "RWA", "646"));

			Children.Create(new Country("BL", "SAINT BARTHELEMY", "Saint Barthelemy", "BLM", "000"));
			Children.Create(new Country("SH", "SAINT HELENA", "Saint Helena", "SHN", "654"));
			Children.Create(new Country("KN", "SAINT KITTS AND NEVIS", "Saint Kitts and Nevis", "KNA", "659"));
			Children.Create(new Country("LC", "SAINT LUCIA", "Saint Lucia", "LCA", "662"));
			Children.Create(new Country("MF", "SAINT MARTIN (FRENCH PART)", "Saint Martin (French Part)", "MAF", "663"));
			Children.Create(new Country("PM", "SAINT PIERRE AND MIQUELON", "Saint Pierre and Miquelon", "SPM", "666"));
			Children.Create(new Country("VC", "SAINT VINCENT AND THE GRENADINES", "Saint Vincent and the Grenadines", "VCT", "670"));
			Children.Create(new Country("WS", "SAMOA", "Samoa", "WSM", "882"));
			Children.Create(new Country("SM", "SAN MARINO", "San Marino", "SMR", "674"));
			Children.Create(new Country("ST", "SAO TOME AND PRINCIPE", "Sao Tome and Principe", "STP", "678"));
			Children.Create(new Country("SA", "SAUDI ARABIA", "Saudi Arabia", "SAU", "682"));
			Children.Create(new Country("SN", "SENEGAL", "Senegal", "SEN", "686"));
			Children.Create(new Country("RS", "SERBIA", "Serbia", "SRB", "688"));
			Children.Create(new Country("SC", "SEYCHELLES", "Seychelles", "SYC", "690"));
			Children.Create(new Country("SL", "SIERRA LEONE", "Sierra Leone", "SLE", "694"));
			Children.Create(new Country("SG", "SINGAPORE", "Singapore", "SGP", "702"));
			Children.Create(new Country("SX", "SINT MAARTEN (DUTCH PART)", "Sint Martin (Dutch Part)", "SXM", "703"));
			Children.Create(new Country("SK", "SLOVAKIA", "Slovakia", "SVK", "703"));
			Children.Create(new Country("SI", "SLOVENIA", "Slovenia", "SVN", "705"));
			Children.Create(new Country("SB", "SOLOMON ISLANDS", "Solomon Islands", "SLB", "090"));
			Children.Create(new Country("SO", "SOMALIA", "Somalia", "SOM", "706"));
			Children.Create(new Country("ZA", "SOUTH AFRICA", "South Africa", "ZAF", "710"));
			Children.Create(new Country("GS", "SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS", "South Georgia and the South Sandwich Islands", "SGS", "239"));
			Children.Create(new Country("ES", "SPAIN", "Spain", "ESP", "724"));
			Children.Create(new Country("LK", "SRI LANKA", "Sri Lanka", "LKA", "144"));
			Children.Create(new Country("SD", "SUDAN", "Sudan", "SDN", "736"));
			Children.Create(new Country("SR", "SURINAME", "Suriname", "SUR", "740"));
			Children.Create(new Country("SJ", "SVALBARD AND JAN MAYEN", "Svalbard and Jan Mayen", "SJM", "744"));
			Children.Create(new Country("SZ", "SWAZILAND", "Swaziland", "SWZ", "748"));
			Children.Create(new Country("SE", "SWEDEN", "Sweden", "SWE", "752"));
			Children.Create(new Country("CH", "SWITZERLAND", "Switzerland", "CHE", "756"));
			Children.Create(new Country("SY", "SYRIAN ARAB REPUBLIC", "Syrian Arab Republic", "SYR", "760"));

			Children.Create(new Country("TW", "TAIWAN, PROVINCE OF CHINA", "Taiwan, Province of China", "TWN", "158"));
			Children.Create(new Country("TJ", "TAJIKISTAN", "Tajikistan", "TJK", "762"));
			Children.Create(new Country("TZ", "TANZANIA, UNITED REPUBLIC OF", "Tanzania, United Republic of", "TZA", "834"));
			Children.Create(new Country("TH", "THAILAND", "Thailand", "THA", "764"));
			Children.Create(new Country("TL", "TIMOR-LESTE", "Timor-Leste", "TLS", "626"));
			Children.Create(new Country("TG", "TOGO", "Togo", "TGO", "768"));
			Children.Create(new Country("TK", "TOKELAU", "Tokelau", "TKL", "772"));
			Children.Create(new Country("TO", "TONGA", "Tonga", "TON", "776"));
			Children.Create(new Country("TT", "TRINIDAD AND TOBAGO", "Trinidad and Tobago", "TTO", "780"));
			Children.Create(new Country("TN", "TUNISIA", "Tunisia", "TUN", "788"));
			Children.Create(new Country("TR", "TURKEY", "Turkey", "TUR", "792"));
			Children.Create(new Country("TM", "TURKMENISTAN", "Turkmenistan", "TKM", "795"));
			Children.Create(new Country("TC", "TURKS AND CAICOS ISLANDS", "Turks and Caicos Islands", "TCA", "796"));
			Children.Create(new Country("TV", "TUVALU", "Tuvalu", "TUV", "798"));

			Children.Create(new Country("UG", "UGANDA", "Uganda", "UGA", "800"));
			Children.Create(new Country("UA", "UKRAINE", "Ukraine", "UKR", "804"));
			Children.Create(new Country("AE", "UNITED ARAB EMIRATES", "United Arab Emirates", "ARE", "784"));
			Children.Create(new Country("GB", "UNITED KINGDOM", "United Kingdom", "GBR", "826"));
			Children.Create(new Country("US", "UNITED STATES", "United States", "USA", "840"));
			Children.Create(new Country("UM", "UNITED STATES MINOR OUTLYING ISLANDS", "United States Minor Outlying Islands", "UMI", "581"));
			Children.Create(new Country("UY", "URUGUAY", "Uruguay", "URY", "858"));
			Children.Create(new Country("UZ", "UZBEKISTAN", "Uzbekistan", "UZB", "860"));

			Children.Create(new Country("VU", "VANUATU", "Vanuatu", "VUT", "548"));
			Children.Create(new Country("VE", "VENEZUELA", "Venezuela", "VEN", "862"));
			Children.Create(new Country("VN", "VIET NAM", "Viet Nam", "VNM", "704"));
			Children.Create(new Country("VG", "VIRGIN ISLANDS, BRITISH", "Virgin Islands, British", "VGB", "092"));
			Children.Create(new Country("VI", "VIRGIN ISLANDS, U.S.", "Virgin Islands, U.s.", "VIR", "850"));

			Children.Create(new Country("WF", "WALLIS AND FUTUNA", "Wallis and Futuna", "WLF", "876"));
			Children.Create(new Country("EH", "WESTERN SAHARA", "Western Sahara", "ESH", "732"));

			Children.Create(new Country("YE", "YEMEN", "Yemen", "YEM", "887"));

			Children.Create(new Country("ZM", "ZAMBIA", "Zambia", "ZMB", "894"));
			Children.Create(new Country("ZW", "ZIMBABWE", "Zimbabwe", "ZWE", "716"));

			base.OnAfterCreate();
		}
	}
}