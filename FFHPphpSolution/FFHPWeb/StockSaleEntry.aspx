<%@ Page Title="" Language="C#" MasterPageFile="~/FFHP.Master" AutoEventWireup="true"
    CodeBehind="StockSaleEntry.aspx.cs" Inherits="FFHPWeb.StockSaleEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <script src="Scripts/ScrollableTablePlugin_1.0_min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $('#Table1').Scrollable({
                ScrollHeight: 350
            });

        });
    </script>

    <%--<script src="http://ajax.aspnetcdn.com/ajax/jquery/jquery-1.8.0.js"></script>  --%>

    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.22/jquery-ui.js"></script>

    <link rel="Stylesheet" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.10/themes/redmond/jquery-ui.css" />

    <script type="text/javascript">
        $(function() {
            //////        var dataSrc = ["Agathi Keerai அகத்தி கீரை", "Abutilon indicum துத்திக் கீரை", "Amaranth முளைக்கீரை", "Amaranthus-Organic அரைகீரை", "Amaranthus Tender தண்டுகீரை", "Amaranthus Tricolor அரைகீரை", "Ballon Vines-Destem முடக்கத்தான்கீரை", "Black Nightshade மணத்தக்காளிகீரை", "Black Nightshade-seed மணத்தக்காளிக்காய்", "Drumstick Leaves முருங்கை கீரை", "Green Amaranthus Tender-Organic தண்டுகீரை", "Henna Leaf மருதாணி", "Hibiscus Flower செம்பருத்தி", "Hibiscus Leaf செம்பருத்தி", "Indian Pennywort-Destem வல்லாரை", "Keela Nelli கீழா நெல்லி", "Kuppaimeni குப்பைமேனி", "Lambsquarters சக்கரவர்த்தி கீரை", "Paruppu keerai பருப்பு கீரை", "Purple Lippia பொட்டுத்தலை", "Ponnanganni Keerai பொன்னாங்கண்ணிகீரை", "White Karisalankanni வெள்ளை கரிசலாங்கண்ணி கீரை", "Red Amaranth சிவப்பு முளைக்கீரை", "Red Amaranthus Tender-Organic தண்டுகீரை", "ThaiNightShade-Destem தூதுவளை", "Radish-Organic முள்ளங்கி", "Red Ponnanganni Keerai சிவப்பு பொன்னாங்கண்ணி கீரை", "Silver cocks comb பண்ணை கீரை", "Sorrel Leaves-Organic புளிச்சகீரை", "Yellow Karisalankanni மஞ்சள் கரிசலாங்கண்ணி கீரை", "Sorrel Leaves புளிச்சகீரை", "Tanners Cassia ஆவாரம்பூ", "Tropical Amaranth-Organic சிறுகீரை", "Tropical Amaranth சிறுகீரை", "Vadhanarayana keerai வாதநாராயண கீரை", "Veldt Grape பிரண்டை", "Vendhaya Keerai வெந்தயக் கீரை", "Spinach green பசலைகீரை", "Spring onion வெங்காயத்தாள்", "Sukkan Keerai சுக்கான் கீரை", "Palak பாலக்கீரை", "Musumusukkai Keerai முசுமுசுக்கை கீரை", "Agathi Keerai-Organic அகத்தி கீரை", "Amla-Organic நெல்லி", "Banana Karpuram-Organic கற்பூரவாழை", "Banana Pith-Organic வாழைத்தண்டு", "Banana Rasthali-Organic ரஸ்தாலி", "Banana Red-Organic செவ்வாழை", "Banana Poovan-Organic பூவன்வாழை ", "Bottle Gourd-Organic சுரக்காய்", "Brinjal Ujala-Organic உஜாலாகத்தரி", "Brinjal Vari-Organic வரிகத்தரி", "Brinjal Tube Purple-Organic டியூப்கத்தரி", "Brinjal Tube Green-Organic டியூப்கத்தரி", "Bush Beans-Organic பீன்ஸ்", "Carrot Purple-Organic கேரட்", "Carrot Red-Organic கேரட்", "Carrot White-Organic கேரட்", "Coriander-Organic கொத்தமல்லி", "Drumstick-Organic முருங்கை", "Greenchilli-Organic மிளகாய்", "Guava-Organic கொய்யா", "Ivy Gourd-Organic கோவக்காய்", "Kale-Organic", "Ladiesfinger-Organic வெண்டைக்காய்", "Palak-Organic பாலக்கீரை", "Papaya Red Lady-Organic பப்பாளி", "Ponnanganni Keerai-Organic பொன்னாங்கண்ணிகீரை", "Swiss Chard-Organic", "Watermelon-Organic தர்பூசணி", "WheatGrass கோதுமைபுல்", "Yellow Pumpkin-Organic மஞ்சள்பூசணி", "Asparagus அஸ்பாரகஸ்", "Amla நெல்லி", "Ash Gourd வெள்ளைபூசணி", "Baby Corn-Raw இளஞ்சோளம்", "Banana Flower வாழைப்பூ", "Beetroot பீட்ருட்", "Big Onion பெரியவெங்காயம்", "Big Onion Grade B பெரியவெங்காயம்", "Bitter Gourd பாகற்காய்", "Baby Bitter Gourd சிறியபாகற்காய்", "Bottle Gourd சுரக்காய்", "Brinjal Disco டிஸ்கோகத்தரி", "Brinjal Palayam பாளையம்கத்தரி", "Brinjal Subal சுபால்கத்தரி", "Brinjal Tube டியூப்கத்தரி", "Brinjal Ujala உஜாலாகத்தரி", "Brinjal Vari வரிகத்தரி", "Brinjal-White வெள்ளைகத்தரி", "Broad Beans அவரை", "Broad Beans-Country நாட்டுஅவரை", "Butter Beans பட்டர்பீன்ஸ்", "Cabbage முட்டைக்கோஸ்", "Capsicum Green பச்சைகுடைமிளகாய்", "Capsicum Red சிவப்புகுடைமிளகாய்", "Capsicum Yellow மஞ்சள்குடைமிளகாய்", "Carrot கேரட்", "Carrot Delhi கேரட்", "Cauliflower காலிஃபிளவர்", "Chilli Bajji பஜ்ஜிமிளகாய்", "Chowchow சவ்சவ்", "Cluster Beans கொத்தவரக்காய்", "Coconut-Organic தேங்காய்", "Coconut தேங்காய்", "Coconut medium தேங்காய்", "Chinese potato சிறுகிழங்கு", "Cucumber வெள்ளரி", "Dosakai தோசை காய்", "Double Beans டபுள்பீன்ஸ்", "Double Beans-Peeled டபுள்பீன்ஸ்", "Drumstick முருங்கை", "Field beans மொச்சை", "Garlic Country நாட்டுப்பூண்டு", "Garlic Malai மலைப்பூண்டு", "Garlic Uthiri உதிரிபூண்டு", "Garlic Kodai Grade A பூண்டு", "Garlic Kodai Grade B பூண்டு", "Garlic Solo Kodaikanal பூண்டு", "Garlic Himachal பூண்டு", "Garlic Himachal-splittedபூண்டு", "Ginger Fresh இஞ்சி", "Ginger இஞ்சி", "Green Peas பச்சைபட்டாணி", "Greenchilli மிளகாய்", "Greenchilli-Country மிளகாய்", "Ground nut நிலக்கடலை", "Ivy Gourd கோவக்காய்", "Raw Jack Fruit பலாக்காய்", "Kankoda கன்கோடா", "Turnip நூல்கோல்", "Ladiesfinger வெண்டைக்காய்", "Lemon small சிறியஎலுமிச்சை", "Lemon-Big பெரியஎலுமிச்சை", "Long Beans காராமணி", "Lotus Stem", "Lucky Tiger-Tomato", "Malabar Cucumber மலபார் வெள்ளரிக்காய்", "Manila Tamarind கொடுக்காப்புளி", "Ooty Cucumber ஊட்டிவெள்ளரி", "Parwal பர்வல்", "Pigeon Pea துவரை", "Pidi Karunai கருணைக்கிழங்கு", "Pinju vellari பிஞ்சு வெள்ளரி", "Potato உருளைக்கிழங்கு", "Potato Agra உருளைக்கிழங்கு", "Panang Kizhangu பனங்கிழங்கு", "Mango ginger இஞ்சி", "Radish Red முள்ளங்கி", "Radish முள்ளங்கி", "Raw Banana வாழைக்காய்", "Raw Mango மாங்காய்", "Red Cabbage சிவப்பு முட்டைக்கோஸ்", "Red Turnip நூல்கோல்", "Ridge Gourd பீர்கங்காய்", "Ring Beans பீன்ஸ்", "Small Bitter Gourd சிறியபாகற்காய்", "Small Onion சிறியவெங்காயம்", "Snake Gourd புடலங்காய்", "Sweet Corn மக்காச்சோளம்", "Sweet Potato சர்க்கரைவள்ளிகிழங்கு", "Tamarind புளி", "Tapioca மரவள்ளி", "Taro Root சேப்பங்கிழங்கு", "Tender mango வடு மாங்காய்", "Tinda தின்டா", "Tomato-Bangalore பெங்களூர்தக்காளி", "Tomato Country-Organic நாட்டுதக்காளி", "Tomato-Country நாட்டுதக்காளி", "Tomato-Green பச்சை தக்காளி", "Turkey Berry சுண்டைக்காய்", "Turnip-Organic நூல்கோல்", "Unripe Figs அத்தி காய்", "White Beans பீன்ஸ்", "White Onion வெள்ளை வெங்காயம்", "Yam சேனைக்கிழங்கு", "Yellow Pumpkin மஞ்சள்பூசணி", "Apple Washington ஆப்பிள்", "Apple Ambri ஆப்பிள்", "Apple Golden ஆப்பிள்", "Apple Fuji ஆப்பிள்", "Apple Royal Gala ஆப்பிள்", "Apple Shimla ஆப்பிள்", "Avocado வெண்ணெய் பழம்", "Avocado Imported வெண்ணெய் பழம்", "Banana Country நாட்டுவாழை", "Banana Green பச்சை வாழை", "Banana Hills மலை வாழை", "Banana karpuram கற்பூரவாழை", "Banana Nendran நேந்திரவாழை", "Banana poovan பூவன்வாழை ", "Banana Rasthali ரஸ்தாலி", "Banana RED செவ்வாழை", "Banana Yelaki ஏலக்கி", "Banana Morris மோரிஸ்வாழை", "Ber Fruit இலந்தை பழம்", "Custard Apple சீத்தாப்பழம்", "Citroen நாரத்தங்காய்", "Grapes Juice திராட்சை", "Grapes Green Seedless பச்சை திராட்சை ", "Grapes Black Seedless கருப்பு திராட்சை", "Grapes Paneer திராட்சை", "Grapes Red Globe சிவப்பு திராட்சை", "Green Apple USA ஆப்பிள்", "Guava கொய்யா", "Guava pink கொய்யா", "Grapefruit பப்ளிமாசு", "Indian Lychee லிச்சி", "JackFruit பலாப்பழம்", "Jamun நாகப்பழம்", "Longan கடுகுடாப் பழம்", "Kiwi கிவி", "Mulampalam முலாம்பழம்", "Muskmelon கிர்ணிபழம்", "Orange Imported ஆரஞ்சு", "Orange Kodai ஆரஞ்சு", "Orange Malta ஆரஞ்சு", "Orange Nagpur ஆரஞ்சு", "Orange Kamala ஆரஞ்சு", "Orange Kinnow ஆரஞ்சு", "Original Kimia Dates கிமியா பேரிச்சம்பழம்", "Papaya பப்பாளி", "Passion Fruits தாட்பூட்பழம்", "Pears Green பச்சை பேரிக்காய்", "Pears ooty ஊட்டிபேரிக்காய்", "Pears Golden பேரிக்காய்", "Pears Red சிவப்பு பேரிக்காய்", "Pears பேரிக்காய்", "Pineapple அன்னாசி", "Pomegranate மாதுளை", "Pomegranate-Country மாதுளை", "Sapota சப்போட்டா", "Sun melon சன் முலாம்பழம்", "Sweet Lime சாத்துக்குடி", "Watermelon Hybrid தர்பூசணி", "Watermelon தர்பூசணி", "Watermelon Yellow தர்பூசணி", "Water Rose Apple", "Cherries செர்ரி", "Blueberry அவுரிநெல்லி", "Dragon Fruit தறுகண்பழம்", "Durian Fruit முள்நாரிப்பழம்", "Apricot-Exotic 1Kg சர்க்கரை பாதாமி", "Cherry-Exotic 1kg சேலாப்பழம்", "Fresh Dates-Exotic 1Kg பேரிச்சம்பழம்", "Exotic-Mangosteen மங்குஸ்தான்", "Peach-Exotic 1kg குழிப்பேரி", "Exotic-Rambutan 1kg ரம்புட்டான்", "Fresh Fig அத்திப்பழம்", "Plums Imported ஆல்பக்கோடா", "Plums Kodaikanal ஆல்பக்கோடா", "Kaki Persimmon சீமைப் பனிச்சை", "Red Currant செங்கொடிமுந்திரி", "Starfruit விளிம்பிப்பழம்", "Strawberry செம்புற்றுப்பழம்", "Sweet Tamarind இனிப்பு புளி", "Thai Guava கொய்யா", "Yellow dates மஞ்சள் பேரிச்சம்பழம்", "Banana Pith வாழைத்தண்டு", "Beetle Leaf வெற்றிலை", "Coriander கொத்தமல்லி", "Curryleaves கறிவேப்பிலை", "Edu Ilai வாழைஇலை", "Mint புதினா", "Thala Ilai தலைவாழை இலை", "Broccoli ப்ரோக்கோலி", "Brussels sprouts களைக்கோசு", "Basil பசில்", "Celery செலரி", "Chineese Cabbage சீனகோஸ்", "Dill சதகுப்பி", "Leeks லீக்ஸ்", "Lemon Grass", "Lettuce இலைக்கோஸ்", "Lettuce Green இலைக்கோஸ்", "Lettuce Purpleஇலைக்கோஸ்", "Parsley வோக்கோசு", "Bok Choy போக் சோய்", "Tomato -Cherry செர்ரி தக்காளி", "Yellow Zuccini மஞ்சள்சுச்சினி", "Zuccini சுச்சினி", "Button Mushroom காளான்", "Baby Corn-Peeled இளஞ்சோளம்", "Baby Potato சிறியஉருளை", "Black chickpeas Sprouts முளைகட்டிய கொண்டைக் கடலை", "Cowpea Sprouts முளைகட்டிய காராமணி", "Green Gram Sprouts முளைகட்டிய பச்சைபயிறு", "Horse Gram Sprouts முளைகட்டிய கொள்ளு", "Mixed Sprouts கலப்பு முளைகட்டிய பயறு", "Pearl Millet Sprouts முளைகட்டிய கம்பு", "Sorghum Sprouts முளைகட்டிய சோளம்", "Rumani Mango 1 Kg", "Rumani Mango 2.5 Kg", "Banganapalli Mango 2.5 Kgs", "Banganapalli Mango 1 Kg", "Red Kidney Beans பீன்ஸ்", "RamSita சீத்தாப்பழம்", "Potato Mettupalayam உருளைக்கிழங்கு", "Coconut - Suruli Falls தேங்காய்", "Kiwi Golden கிவி", "Insulin Keerai இன்சுலின் கீரை", "Banana Saba பேய் வாழை", "Rosemary ரோஸ்மேரி", "Aloe Vera One Leaf அலோ வேரா "];
            var dataSrc = ["Agathi Keerai அகத்தி கீரை 559", "Abutilon indicum துத்திக் கீரை 640", "Amaranth முளைக்கீரை 340", "Amaranthus-Organic அரைகீரை 479", "Amaranthus Tender தண்டுகீரை 275", "Amaranthus Tricolor அரைகீரை 232", "Ballon Vines-Destem முடக்கத்தான்கீரை 300", "Black Nightshade மணத்தக்காளிகீரை 337", "Black Nightshade-seed மணத்தக்காளிக்காய் 641", "Drumstick Leaves முருங்கை கீரை 569", "Green Amaranthus Tender-Organic தண்டுகீரை 586", "Henna Leaf மருதாணி 638", "Hibiscus Flower செம்பருத்தி 643", "Hibiscus Leaf செம்பருத்தி 644", "Indian Pennywort-Destem வல்லாரை 304", "Keela Nelli கீழா நெல்லி 565", "Kuppaimeni குப்பைமேனி 568", "Lambsquarters சக்கரவர்த்தி கீரை 639", "Paruppu keerai பருப்பு கீரை 558", "Purple Lippia பொட்டுத்தலை 637", "Ponnanganni Keerai பொன்னாங்கண்ணிகீரை 425", "White Karisalankanni வெள்ளை கரிசலாங்கண்ணி கீரை 554", "Red Amaranth சிவப்பு முளைக்கீரை 556", "Red Amaranthus Tender-Organic தண்டுகீரை 449", "ThaiNightShade-Destem தூதுவளை 303", "Radish-Organic முள்ளங்கி 454", "Red Ponnanganni Keerai சிவப்பு பொன்னாங்கண்ணி கீரை 557", "Silver cocks comb பண்ணை கீரை 642", "Sorrel Leaves-Organic புளிச்சகீரை 514", "Yellow Karisalankanni மஞ்சள் கரிசலாங்கண்ணி கீரை 563", "Sorrel Leaves புளிச்சகீரை 341", "Tanners Cassia ஆவாரம்பூ 561", "Tropical Amaranth-Organic சிறுகீரை 448", "Tropical Amaranth சிறுகீரை 339", "Vadhanarayana keerai வாதநாராயண கீரை 566", "Veldt Grape பிரண்டை 349", "Vendhaya Keerai வெந்தயக் கீரை 444", "Spinach green பசலைகீரை 301", "Spring onion வெங்காயத்தாள் 302", "Sukkan Keerai சுக்கான் கீரை 560", "Palak பாலக்கீரை 265", "Musumusukkai Keerai முசுமுசுக்கை கீரை 567", "Agathi Keerai-Organic அகத்தி கீரை 662", "Amla-Organic நெல்லி 655", "Banana Karpuram-Organic கற்பூரவாழை 610", "Banana Pith-Organic வாழைத்தண்டு 664", "Banana Rasthali-Organic ரஸ்தாலி 612", "Banana Red-Organic செவ்வாழை 611", "Banana Poovan-Organic பூவன்வாழை  650", "Bottle Gourd-Organic சுரக்காய் 505", "Brinjal Ujala-Organic உஜாலாகத்தரி 691", "Brinjal Vari-Organic வரிகத்தரி 543", "Brinjal Tube Purple-Organic டியூப்கத்தரி 614", "Brinjal Tube Green-Organic டியூப்கத்தரி 615", "Bush Beans-Organic பீன்ஸ் 453", "Carrot Purple-Organic கேரட் 518", "Carrot Red-Organic கேரட் 519", "Carrot White-Organic கேரட் 520", "Coriander-Organic கொத்தமல்லி 452", "Drumstick-Organic முருங்கை 592", "Greenchilli-Organic மிளகாய் 455", "Guava-Organic கொய்யா 661", "Ivy Gourd-Organic கோவக்காய் 571", "Kale-Organic 504", "Ladiesfinger-Organic வெண்டைக்காய் 456", "Palak-Organic பாலக்கீரை 451", "Papaya Red Lady-Organic பப்பாளி 616", "Ponnanganni Keerai-Organic பொன்னாங்கண்ணிகீரை 679", "Swiss Chard-Organic 469", "Watermelon-Organic தர்பூசணி 680", "WheatGrass கோதுமைபுல் 410", "Yellow Pumpkin-Organic மஞ்சள்பூசணி 447", "Asparagus அஸ்பாரகஸ் 443", "Amla நெல்லி 234", "Ash Gourd வெள்ளைபூசணி 280", "Baby Corn-Raw இளஞ்சோளம் 297", "Banana Flower வாழைப்பூ 284", "Beetroot பீட்ருட் 236", "Big Onion பெரியவெங்காயம் 237", "Big Onion Grade B பெரியவெங்காயம் 594", "Bitter Gourd பாகற்காய் 266", "Baby Bitter Gourd சிறியபாகற்காய் 613", "Bottle Gourd சுரக்காய் 273", "Brinjal Disco டிஸ்கோகத்தரி 358", "Brinjal Palayam பாளையம்கத்தரி 359", "Brinjal Subal சுபால்கத்தரி 240", "Brinjal Tube டியூப்கத்தரி 360", "Brinjal Ujala உஜாலாகத்தரி 239", "Brinjal Vari வரிகத்தரி 238", "Brinjal-White வெள்ளைகத்தரி 685", "Broad Beans அவரை 233", "Broad Beans-Country நாட்டுஅவரை 363", "Butter Beans பட்டர்பீன்ஸ் 286", "Cabbage முட்டைக்கோஸ் 242", "Capsicum Green பச்சைகுடைமிளகாய் 243", "Capsicum Red சிவப்புகுடைமிளகாய் 244", "Capsicum Yellow மஞ்சள்குடைமிளகாய் 245", "Carrot கேரட் 246", "Carrot Delhi கேரட் 656", "Cauliflower காலிஃபிளவர் 696", "Chilli Bajji பஜ்ஜிமிளகாய் 283", "Chowchow சவ்சவ் 272", "Cluster Beans கொத்தவரக்காய் 255", "Coconut-Organic தேங்காய் 484", "Coconut தேங்காய் 247", "Coconut medium தேங்காய் 657", "Chinese potato சிறுகிழங்கு 431", "Cucumber வெள்ளரி 250", "Dosakai தோசை காய் 528", "Double Beans டபுள்பீன்ஸ் 435", "Double Beans-Peeled டபுள்பீன்ஸ் 660", "Drumstick முருங்கை 262", "Field beans மொச்சை 321", "Garlic Country நாட்டுப்பூண்டு 287", "Garlic Malai மலைப்பூண்டு 285", "Garlic Uthiri உதிரிபூண்டு 288", "Garlic Kodai Grade A பூண்டு 665", "Garlic Kodai Grade B பூண்டு 666", "Garlic Solo Kodaikanal பூண்டு 667", "Garlic Himachal பூண்டு 668", "Garlic Himachal-splittedபூண்டு 669", "Ginger Fresh இஞ்சி 365", "Ginger இஞ்சி 252", "Green Peas பச்சைபட்டாணி 409", "Greenchilli மிளகாய் 253", "Greenchilli-Country மிளகாய் 688", "Ground nut நிலக்கடலை 254", "Ivy Gourd கோவக்காய் 289", "Raw Jack Fruit பலாக்காய் 291", "Kankoda கன்கோடா 256", "Turnip நூல்கோல் 264", "Ladiesfinger வெண்டைக்காய் 257", "Lemon small சிறியஎலுமிச்சை 258", "Lemon-Big பெரியஎலுமிச்சை 370", "Long Beans காராமணி 290", "Lotus Stem 593", "Lucky Tiger-Tomato 683", "Malabar Cucumber மலபார் வெள்ளரிக்காய் 532", "Manila Tamarind கொடுக்காப்புளி 494", "Ooty Cucumber ஊட்டிவெள்ளரி 344", "Parwal பர்வல் 269", "Pigeon Pea துவரை 408", "Pidi Karunai கருணைக்கிழங்கு 598", "Pinju vellari பிஞ்சு வெள்ளரி 446", "Potato உருளைக்கிழங்கு 267", "Potato Agra உருளைக்கிழங்கு 652", "Panang Kizhangu பனங்கிழங்கு 424", "Mango ginger இஞ்சி 595", "Radish Red முள்ளங்கி 529", "Radish முள்ளங்கி 261", "Raw Banana வாழைக்காய் 278", "Raw Mango மாங்காய் 292", "Red Cabbage சிவப்பு முட்டைக்கோஸ் 342", "Red Turnip நூல்கோல் 609", "Ridge Gourd பீர்கங்காய் 407", "Ring Beans பீன்ஸ் 235", "Small Bitter Gourd சிறியபாகற்காய் 259", "Small Onion சிறியவெங்காயம் 271", "Snake Gourd புடலங்காய் 268", "Sweet Corn மக்காச்சோளம் 274", "Sweet Potato சர்க்கரைவள்ளிகிழங்கு 294", "Tamarind புளி 295", "Tapioca மரவள்ளி 361", "Taro Root சேப்பங்கிழங்கு 270", "Tender mango வடு மாங்காய் 477", "Tinda தின்டா 687", "Tomato-Bangalore பெங்களூர்தக்காளி 276", "Tomato Country-Organic நாட்டுதக்காளி 544", "Tomato-Country நாட்டுதக்காளி 277", "Tomato-Green பச்சை தக்காளி 686", "Turkey Berry சுண்டைக்காய் 350", "Turnip-Organic நூல்கோல் 513", "Unripe Figs அத்தி காய் 564", "White Beans பீன்ஸ் 372", "White Onion வெள்ளை வெங்காயம் 553", "Yam சேனைக்கிழங்கு 296", "Yellow Pumpkin மஞ்சள்பூசணி 281", "Apple Washington ஆப்பிள் 307", "Apple Ambri ஆப்பிள் 602", "Apple Golden ஆப்பிள் 599", "Apple Fuji ஆப்பிள் 369", "Apple Royal Gala ஆப்பிள் 305", "Apple Shimla ஆப்பிள் 517", "Avocado வெண்ணெய் பழம் 464", "Avocado Imported வெண்ணெய் பழம் 465", "Banana Country நாட்டுவாழை 689", "Banana Green பச்சை வாழை 522", "Banana Hills மலை வாழை 503", "Banana karpuram கற்பூரவாழை 423", "Banana Nendran நேந்திரவாழை 422", "Banana poovan பூவன்வாழை  427", "Banana Rasthali ரஸ்தாலி 581", "Banana RED செவ்வாழை 309", "Banana Yelaki ஏலக்கி 502", "Banana Morris மோரிஸ்வாழை 308", "Ber Fruit இலந்தை பழம் 654", "Custard Apple சீத்தாப்பழம் 618", "Citroen நாரத்தங்காய் 636", "Grapes Juice திராட்சை 310", "Grapes Green Seedless பச்சை திராட்சை  430", "Grapes Black Seedless கருப்பு திராட்சை 429", "Grapes Paneer திராட்சை 605", "Grapes Red Globe சிவப்பு திராட்சை 604", "Green Apple USA ஆப்பிள் 457", "Guava கொய்யா 311", "Guava pink கொய்யா 603", "Grapefruit பப்ளிமாசு 633", "Indian Lychee லிச்சி 694", "JackFruit பலாப்பழம் 506", "Jamun நாகப்பழம் 574", "Longan கடுகுடாப் பழம் 601", "Kiwi கிவி 441", "Mulampalam முலாம்பழம் 527", "Muskmelon கிர்ணிபழம் 319", "Orange Imported ஆரஞ்சு 524", "Orange Kodai ஆரஞ்சு 572", "Orange Malta ஆரஞ்சு 440", "Orange Nagpur ஆரஞ்சு 478", "Orange Kamala ஆரஞ்சு 314", "Orange Kinnow ஆரஞ்சு 653", "Original Kimia Dates கிமியா பேரிச்சம்பழம் 357", "Papaya பப்பாளி 320", "Passion Fruits தாட்பூட்பழம் 495", "Pears Green பச்சை பேரிக்காய் 583", "Pears ooty ஊட்டிபேரிக்காய் 607", "Pears Golden பேரிக்காய் 606", "Pears Red சிவப்பு பேரிக்காய் 582", "Pears பேரிக்காய் 315", "Pineapple அன்னாசி 345", "Pomegranate மாதுளை 317", "Pomegranate-Country மாதுளை 684", "Sapota சப்போட்டா 421", "Sun melon சன் முலாம்பழம் 521", "Sweet Lime சாத்துக்குடி 371", "Watermelon Hybrid தர்பூசணி 346", "Watermelon தர்பூசணி 438", "Watermelon Yellow தர்பூசணி 681", "Water Rose Apple 682", "Cherries செர்ரி 460", "Blueberry அவுரிநெல்லி 459", "Dragon Fruit தறுகண்பழம் 463", "Durian Fruit முள்நாரிப்பழம் 634", "Apricot-Exotic 1Kg சர்க்கரை பாதாமி 549", "Cherry-Exotic 1kg சேலாப்பழம் 547", "Fresh Dates-Exotic 1Kg பேரிச்சம்பழம் 551", "Exotic-Mangosteen மங்குஸ்தான் 313", "Peach-Exotic 1kg குழிப்பேரி 548", "Exotic-Rambutan 1kg ரம்புட்டான் 546", "Fresh Fig அத்திப்பழம் 523", "Plums Imported ஆல்பக்கோடா 466", "Plums Kodaikanal ஆல்பக்கோடா 552", "Kaki Persimmon சீமைப் பனிச்சை 635", "Red Currant செங்கொடிமுந்திரி 458", "Starfruit விளிம்பிப்பழம் 573", "Strawberry செம்புற்றுப்பழம் 461", "Sweet Tamarind இனிப்பு புளி 580", "Thai Guava கொய்யா 462", "Yellow dates மஞ்சள் பேரிச்சம்பழம் 579", "Banana Pith வாழைத்தண்டு 279", "Beetle Leaf வெற்றிலை 355", "Coriander கொத்தமல்லி 248", "Curryleaves கறிவேப்பிலை 251", "Edu Ilai வாழைஇலை 356", "Mint புதினா 260", "Thala Ilai தலைவாழை இலை 354", "Broccoli ப்ரோக்கோலி 241", "Brussels sprouts களைக்கோசு 555", "Basil பசில் 467", "Celery செலரி 335", "Chineese Cabbage சீனகோஸ் 334", "Dill சதகுப்பி 631", "Leeks லீக்ஸ் 336", "Lemon Grass 507", "Lettuce இலைக்கோஸ் 428", "Lettuce Green இலைக்கோஸ் 617", "Lettuce Purpleஇலைக்கோஸ் 480", "Parsley வோக்கோசு 623", "Bok Choy போக் சோய் 693", "Tomato -Cherry செர்ரி தக்காளி 508", "Yellow Zuccini மஞ்சள்சுச்சினி 368", "Zuccini சுச்சினி 343", "Button Mushroom காளான் 406", "Baby Corn-Peeled இளஞ்சோளம் 298", "Baby Potato சிறியஉருளை 299", "Black chickpeas Sprouts முளைகட்டிய கொண்டைக் கடலை 472", "Cowpea Sprouts முளைகட்டிய காராமணி 473", "Green Gram Sprouts முளைகட்டிய பச்சைபயிறு 475", "Horse Gram Sprouts முளைகட்டிய கொள்ளு 471", "Mixed Sprouts கலப்பு முளைகட்டிய பயறு 474", "Pearl Millet Sprouts முளைகட்டிய கம்பு 476", "Sorghum Sprouts முளைகட்டிய சோளம் 690", "Rumani Mango 1 Kg 578", "Rumani Mango 2.5 Kg 576", "Banganapalli Mango 2.5 Kgs 695", "Banganapalli Mango 1 Kg 545", "Red Kidney Beans பீன்ஸ் 699", "RamSita சீத்தாப்பழம் 701", "Potato Mettupalayam உருளைக்கிழங்கு 705", "Coconut - Suruli Falls தேங்காய் 725", "Kiwi Golden கிவி 729", "Insulin Keerai இன்சுலின் கீரை 663", "Banana Saba பேய் வாழை 627", "Rosemary ரோஸ்மேரி 773", "Aloe Vera One Leaf அலோ வேரா  282"];
            $("#myText").autocomplete({
                source: dataSrc
            });
        });

        function btnupload_Click() {
            var strconfirm = confirm("Are you sure you want to upload?");
            if (strconfirm == true) {
                return true;
            }
            else {
                return false;
            }
        }
        function btndownload_Click() {
            var strconfirm = confirm("Are you sure you want to download?");
            if (strconfirm == true) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>

    <table width="100%" align="center">
        <tr>
            <td class="heading">
                Total Weight Upload
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td width="25%">
                            <asp:FileUpload ID="FUtotalweight" runat="server" />
                        </td>
                        <td width="25%">
                            <asp:Button ID="Btnupload" Text="Upload" runat="server" OnClick="Btnupload_OnClick"
                                OnClientClick="return btnupload_Click();" />
                        </td>
                        <td align="left" width="50%">
                        <asp:Label ID="lbltotalweightuploadstatus" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblerror" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="heading">
                Stock and Sale Entry
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            Stock
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstocktype" AutoPostBack="true" OnSelectedIndexChanged="ddlstocktype_OnSelectedIndexChanged"
                                runat="server">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Morning" Value="morning"></asp:ListItem>
                                <asp:ListItem Text="Local Purchase" Value="local purchase"></asp:ListItem>
                                <asp:ListItem Text="Balance" Value="balance"></asp:ListItem>
                                <asp:ListItem Text="Local Sale" Value="local sale"></asp:ListItem>
                                <asp:ListItem Text="After Sale" Value="after sale"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <a href="javascript:void(0);" id="get">Get</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Repeater ID="rptstocksale" runat="server">
                    <HeaderTemplate>
                        <table id="Table1" cellspacing="0" rules="all" border="1">
                            <tr bgcolor="#0071BD" style="background-color: #0071BD; font-weight: bold; color: White;">
                                <th scope="col">
                                    Product Id
                                </th>
                                <th scope="col">
                                    Name
                                </th>
                                <th scope="col">
                                    Morning Wt
                                </th>
                                <th scope="col">
                                    Morning Pc
                                </th>
                                <th scope="col">
                                    Tray Wt
                                </th>
                                <th scope="col">
                                    description
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("productid") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblmorningscannedweight" runat="server" Text='<%# Eval("morningscannedweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lblmorningpiececount" runat="server" Text='<%# Eval("morningpiececount") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lbllocalpurchasescannedweight" runat="server" Text='<%# Eval("morningtrayweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lbllocalpurchasepiececount" runat="server" Text='<%# Eval("morningdescription") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblproductid" runat="server" Text='<%# Eval("productid") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblmorningscannedweight" runat="server" Text='<%# Eval("morningscannedweight") %>' />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:Label ID="lblmorningpiececount" runat="server" Text='<%# Eval("morningpiececount") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lbllocalpurchasescannedweight" runat="server" Text='<%# Eval("morningtrayweight") %>' />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:Label ID="lbllocalpurchasepiececount" runat="server" Text='<%# Eval("morningdescription") %>' />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        <tr>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:TextBox ID="txtproductid" runat="server" />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <input id="myText" style="width: 100%" />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:TextBox ID="txtmorningscannedweight" runat="server" />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463;">
                                <asp:TextBox ID="txtmorningpiececount" runat="server" />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <asp:TextBox ID="txtmorningtrayweight" runat="server" />
                            </td>
                            <td style="background-color: #F7F7DE; color: #212463;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtmorningdescription" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnsubmit" Text="Submit" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="GVstock" runat="server">
                </asp:GridView>
            </td>
        </tr>
        <%--<tr>
<td>


<script>
    var dataSrc = ["Agathi Keerai அகத்தி கீரை 559", "Abutilon indicum துத்திக் கீரை 640", "Amaranth முளைக்கீரை 340", "Amaranthus-Organic அரைகீரை 479", "Amaranthus Tender தண்டுகீரை 275", "Amaranthus Tricolor அரைகீரை 232", "Ballon Vines-Destem முடக்கத்தான்கீரை 300", "Black Nightshade மணத்தக்காளிகீரை 337", "Black Nightshade-seed மணத்தக்காளிக்காய் 641", "Drumstick Leaves முருங்கை கீரை 569", "Green Amaranthus Tender-Organic தண்டுகீரை 586", "Henna Leaf மருதாணி 638", "Hibiscus Flower செம்பருத்தி 643", "Hibiscus Leaf செம்பருத்தி 644", "Indian Pennywort-Destem வல்லாரை 304", "Keela Nelli கீழா நெல்லி 565", "Kuppaimeni குப்பைமேனி 568", "Lambsquarters சக்கரவர்த்தி கீரை 639", "Paruppu keerai பருப்பு கீரை 558", "Purple Lippia பொட்டுத்தலை 637", "Ponnanganni Keerai பொன்னாங்கண்ணிகீரை 425", "White Karisalankanni வெள்ளை கரிசலாங்கண்ணி கீரை 554", "Red Amaranth சிவப்பு முளைக்கீரை 556", "Red Amaranthus Tender-Organic தண்டுகீரை 449", "ThaiNightShade-Destem தூதுவளை 303", "Radish-Organic முள்ளங்கி 454", "Red Ponnanganni Keerai சிவப்பு பொன்னாங்கண்ணி கீரை 557", "Silver cocks comb பண்ணை கீரை 642", "Sorrel Leaves-Organic புளிச்சகீரை 514", "Yellow Karisalankanni மஞ்சள் கரிசலாங்கண்ணி கீரை 563", "Sorrel Leaves புளிச்சகீரை 341", "Tanners Cassia ஆவாரம்பூ 561", "Tropical Amaranth-Organic சிறுகீரை 448", "Tropical Amaranth சிறுகீரை 339", "Vadhanarayana keerai வாதநாராயண கீரை 566", "Veldt Grape பிரண்டை 349", "Vendhaya Keerai வெந்தயக் கீரை 444", "Spinach green பசலைகீரை 301", "Spring onion வெங்காயத்தாள் 302", "Sukkan Keerai சுக்கான் கீரை 560", "Palak பாலக்கீரை 265", "Musumusukkai Keerai முசுமுசுக்கை கீரை 567", "Agathi Keerai-Organic அகத்தி கீரை 662", "Amla-Organic நெல்லி 655", "Banana Karpuram-Organic கற்பூரவாழை 610", "Banana Pith-Organic வாழைத்தண்டு 664", "Banana Rasthali-Organic ரஸ்தாலி 612", "Banana Red-Organic செவ்வாழை 611", "Banana Poovan-Organic பூவன்வாழை  650", "Bottle Gourd-Organic சுரக்காய் 505", "Brinjal Ujala-Organic உஜாலாகத்தரி 691", "Brinjal Vari-Organic வரிகத்தரி 543", "Brinjal Tube Purple-Organic டியூப்கத்தரி 614", "Brinjal Tube Green-Organic டியூப்கத்தரி 615", "Bush Beans-Organic பீன்ஸ் 453", "Carrot Purple-Organic கேரட் 518", "Carrot Red-Organic கேரட் 519", "Carrot White-Organic கேரட் 520", "Coriander-Organic கொத்தமல்லி 452", "Drumstick-Organic முருங்கை 592", "Greenchilli-Organic மிளகாய் 455", "Guava-Organic கொய்யா 661", "Ivy Gourd-Organic கோவக்காய் 571", "Kale-Organic 504", "Ladiesfinger-Organic வெண்டைக்காய் 456", "Palak-Organic பாலக்கீரை 451", "Papaya Red Lady-Organic பப்பாளி 616", "Ponnanganni Keerai-Organic பொன்னாங்கண்ணிகீரை 679", "Swiss Chard-Organic 469", "Watermelon-Organic தர்பூசணி 680", "WheatGrass கோதுமைபுல் 410", "Yellow Pumpkin-Organic மஞ்சள்பூசணி 447", "Asparagus அஸ்பாரகஸ் 443", "Amla நெல்லி 234", "Ash Gourd வெள்ளைபூசணி 280", "Baby Corn-Raw இளஞ்சோளம் 297", "Banana Flower வாழைப்பூ 284", "Beetroot பீட்ருட் 236", "Big Onion பெரியவெங்காயம் 237", "Big Onion Grade B பெரியவெங்காயம் 594", "Bitter Gourd பாகற்காய் 266", "Baby Bitter Gourd சிறியபாகற்காய் 613", "Bottle Gourd சுரக்காய் 273", "Brinjal Disco டிஸ்கோகத்தரி 358", "Brinjal Palayam பாளையம்கத்தரி 359", "Brinjal Subal சுபால்கத்தரி 240", "Brinjal Tube டியூப்கத்தரி 360", "Brinjal Ujala உஜாலாகத்தரி 239", "Brinjal Vari வரிகத்தரி 238", "Brinjal-White வெள்ளைகத்தரி 685", "Broad Beans அவரை 233", "Broad Beans-Country நாட்டுஅவரை 363", "Butter Beans பட்டர்பீன்ஸ் 286", "Cabbage முட்டைக்கோஸ் 242", "Capsicum Green பச்சைகுடைமிளகாய் 243", "Capsicum Red சிவப்புகுடைமிளகாய் 244", "Capsicum Yellow மஞ்சள்குடைமிளகாய் 245", "Carrot கேரட் 246", "Carrot Delhi கேரட் 656", "Cauliflower காலிஃபிளவர் 696", "Chilli Bajji பஜ்ஜிமிளகாய் 283", "Chowchow சவ்சவ் 272", "Cluster Beans கொத்தவரக்காய் 255", "Coconut-Organic தேங்காய் 484", "Coconut தேங்காய் 247", "Coconut medium தேங்காய் 657", "Chinese potato சிறுகிழங்கு 431", "Cucumber வெள்ளரி 250", "Dosakai தோசை காய் 528", "Double Beans டபுள்பீன்ஸ் 435", "Double Beans-Peeled டபுள்பீன்ஸ் 660", "Drumstick முருங்கை 262", "Field beans மொச்சை 321", "Garlic Country நாட்டுப்பூண்டு 287", "Garlic Malai மலைப்பூண்டு 285", "Garlic Uthiri உதிரிபூண்டு 288", "Garlic Kodai Grade A பூண்டு 665", "Garlic Kodai Grade B பூண்டு 666", "Garlic Solo Kodaikanal பூண்டு 667", "Garlic Himachal பூண்டு 668", "Garlic Himachal-splittedபூண்டு 669", "Ginger Fresh இஞ்சி 365", "Ginger இஞ்சி 252", "Green Peas பச்சைபட்டாணி 409", "Greenchilli மிளகாய் 253", "Greenchilli-Country மிளகாய் 688", "Ground nut நிலக்கடலை 254", "Ivy Gourd கோவக்காய் 289", "Raw Jack Fruit பலாக்காய் 291", "Kankoda கன்கோடா 256", "Turnip நூல்கோல் 264", "Ladiesfinger வெண்டைக்காய் 257", "Lemon small சிறியஎலுமிச்சை 258", "Lemon-Big பெரியஎலுமிச்சை 370", "Long Beans காராமணி 290", "Lotus Stem 593", "Lucky Tiger-Tomato 683", "Malabar Cucumber மலபார் வெள்ளரிக்காய் 532", "Manila Tamarind கொடுக்காப்புளி 494", "Ooty Cucumber ஊட்டிவெள்ளரி 344", "Parwal பர்வல் 269", "Pigeon Pea துவரை 408", "Pidi Karunai கருணைக்கிழங்கு 598", "Pinju vellari பிஞ்சு வெள்ளரி 446", "Potato உருளைக்கிழங்கு 267", "Potato Agra உருளைக்கிழங்கு 652", "Panang Kizhangu பனங்கிழங்கு 424", "Mango ginger இஞ்சி 595", "Radish Red முள்ளங்கி 529", "Radish முள்ளங்கி 261", "Raw Banana வாழைக்காய் 278", "Raw Mango மாங்காய் 292", "Red Cabbage சிவப்பு முட்டைக்கோஸ் 342", "Red Turnip நூல்கோல் 609", "Ridge Gourd பீர்கங்காய் 407", "Ring Beans பீன்ஸ் 235", "Small Bitter Gourd சிறியபாகற்காய் 259", "Small Onion சிறியவெங்காயம் 271", "Snake Gourd புடலங்காய் 268", "Sweet Corn மக்காச்சோளம் 274", "Sweet Potato சர்க்கரைவள்ளிகிழங்கு 294", "Tamarind புளி 295", "Tapioca மரவள்ளி 361", "Taro Root சேப்பங்கிழங்கு 270", "Tender mango வடு மாங்காய் 477", "Tinda தின்டா 687", "Tomato-Bangalore பெங்களூர்தக்காளி 276", "Tomato Country-Organic நாட்டுதக்காளி 544", "Tomato-Country நாட்டுதக்காளி 277", "Tomato-Green பச்சை தக்காளி 686", "Turkey Berry சுண்டைக்காய் 350", "Turnip-Organic நூல்கோல் 513", "Unripe Figs அத்தி காய் 564", "White Beans பீன்ஸ் 372", "White Onion வெள்ளை வெங்காயம் 553", "Yam சேனைக்கிழங்கு 296", "Yellow Pumpkin மஞ்சள்பூசணி 281", "Apple Washington ஆப்பிள் 307", "Apple Ambri ஆப்பிள் 602", "Apple Golden ஆப்பிள் 599", "Apple Fuji ஆப்பிள் 369", "Apple Royal Gala ஆப்பிள் 305", "Apple Shimla ஆப்பிள் 517", "Avocado வெண்ணெய் பழம் 464", "Avocado Imported வெண்ணெய் பழம் 465", "Banana Country நாட்டுவாழை 689", "Banana Green பச்சை வாழை 522", "Banana Hills மலை வாழை 503", "Banana karpuram கற்பூரவாழை 423", "Banana Nendran நேந்திரவாழை 422", "Banana poovan பூவன்வாழை  427", "Banana Rasthali ரஸ்தாலி 581", "Banana RED செவ்வாழை 309", "Banana Yelaki ஏலக்கி 502", "Banana Morris மோரிஸ்வாழை 308", "Ber Fruit இலந்தை பழம் 654", "Custard Apple சீத்தாப்பழம் 618", "Citroen நாரத்தங்காய் 636", "Grapes Juice திராட்சை 310", "Grapes Green Seedless பச்சை திராட்சை  430", "Grapes Black Seedless கருப்பு திராட்சை 429", "Grapes Paneer திராட்சை 605", "Grapes Red Globe சிவப்பு திராட்சை 604", "Green Apple USA ஆப்பிள் 457", "Guava கொய்யா 311", "Guava pink கொய்யா 603", "Grapefruit பப்ளிமாசு 633", "Indian Lychee லிச்சி 694", "JackFruit பலாப்பழம் 506", "Jamun நாகப்பழம் 574", "Longan கடுகுடாப் பழம் 601", "Kiwi கிவி 441", "Mulampalam முலாம்பழம் 527", "Muskmelon கிர்ணிபழம் 319", "Orange Imported ஆரஞ்சு 524", "Orange Kodai ஆரஞ்சு 572", "Orange Malta ஆரஞ்சு 440", "Orange Nagpur ஆரஞ்சு 478", "Orange Kamala ஆரஞ்சு 314", "Orange Kinnow ஆரஞ்சு 653", "Original Kimia Dates கிமியா பேரிச்சம்பழம் 357", "Papaya பப்பாளி 320", "Passion Fruits தாட்பூட்பழம் 495", "Pears Green பச்சை பேரிக்காய் 583", "Pears ooty ஊட்டிபேரிக்காய் 607", "Pears Golden பேரிக்காய் 606", "Pears Red சிவப்பு பேரிக்காய் 582", "Pears பேரிக்காய் 315", "Pineapple அன்னாசி 345", "Pomegranate மாதுளை 317", "Pomegranate-Country மாதுளை 684", "Sapota சப்போட்டா 421", "Sun melon சன் முலாம்பழம் 521", "Sweet Lime சாத்துக்குடி 371", "Watermelon Hybrid தர்பூசணி 346", "Watermelon தர்பூசணி 438", "Watermelon Yellow தர்பூசணி 681", "Water Rose Apple 682", "Cherries செர்ரி 460", "Blueberry அவுரிநெல்லி 459", "Dragon Fruit தறுகண்பழம் 463", "Durian Fruit முள்நாரிப்பழம் 634", "Apricot-Exotic 1Kg சர்க்கரை பாதாமி 549", "Cherry-Exotic 1kg சேலாப்பழம் 547", "Fresh Dates-Exotic 1Kg பேரிச்சம்பழம் 551", "Exotic-Mangosteen மங்குஸ்தான் 313", "Peach-Exotic 1kg குழிப்பேரி 548", "Exotic-Rambutan 1kg ரம்புட்டான் 546", "Fresh Fig அத்திப்பழம் 523", "Plums Imported ஆல்பக்கோடா 466", "Plums Kodaikanal ஆல்பக்கோடா 552", "Kaki Persimmon சீமைப் பனிச்சை 635", "Red Currant செங்கொடிமுந்திரி 458", "Starfruit விளிம்பிப்பழம் 573", "Strawberry செம்புற்றுப்பழம் 461", "Sweet Tamarind இனிப்பு புளி 580", "Thai Guava கொய்யா 462", "Yellow dates மஞ்சள் பேரிச்சம்பழம் 579", "Banana Pith வாழைத்தண்டு 279", "Beetle Leaf வெற்றிலை 355", "Coriander கொத்தமல்லி 248", "Curryleaves கறிவேப்பிலை 251", "Edu Ilai வாழைஇலை 356", "Mint புதினா 260", "Thala Ilai தலைவாழை இலை 354", "Broccoli ப்ரோக்கோலி 241", "Brussels sprouts களைக்கோசு 555", "Basil பசில் 467", "Celery செலரி 335", "Chineese Cabbage சீனகோஸ் 334", "Dill சதகுப்பி 631", "Leeks லீக்ஸ் 336", "Lemon Grass 507", "Lettuce இலைக்கோஸ் 428", "Lettuce Green இலைக்கோஸ் 617", "Lettuce Purpleஇலைக்கோஸ் 480", "Parsley வோக்கோசு 623", "Bok Choy போக் சோய் 693", "Tomato -Cherry செர்ரி தக்காளி 508", "Yellow Zuccini மஞ்சள்சுச்சினி 368", "Zuccini சுச்சினி 343", "Button Mushroom காளான் 406", "Baby Corn-Peeled இளஞ்சோளம் 298", "Baby Potato சிறியஉருளை 299", "Black chickpeas Sprouts முளைகட்டிய கொண்டைக் கடலை 472", "Cowpea Sprouts முளைகட்டிய காராமணி 473", "Green Gram Sprouts முளைகட்டிய பச்சைபயிறு 475", "Horse Gram Sprouts முளைகட்டிய கொள்ளு 471", "Mixed Sprouts கலப்பு முளைகட்டிய பயறு 474", "Pearl Millet Sprouts முளைகட்டிய கம்பு 476", "Sorghum Sprouts முளைகட்டிய சோளம் 690", "Rumani Mango 1 Kg 578", "Rumani Mango 2.5 Kg 576", "Banganapalli Mango 2.5 Kgs 695", "Banganapalli Mango 1 Kg 545", "Red Kidney Beans பீன்ஸ் 699", "RamSita சீத்தாப்பழம் 701", "Potato Mettupalayam உருளைக்கிழங்கு 705", "Coconut - Suruli Falls தேங்காய் 725", "Kiwi Golden கிவி 729", "Insulin Keerai இன்சுலின் கீரை 663", "Banana Saba பேய் வாழை 627", "Rosemary ரோஸ்மேரி 773", "Aloe Vera One Leaf அலோ வேரா  282"];

    $(document).ready(function() {
        $(".addCF").click(function() {
//        $("#customFields").append('<tr valign="top"><th scope="row"><label for="customFieldName">Custom Field</label></th><td><input id="myText" style="width:100%" class="code" /> &nbsp; <input type="text" id="myText2" /> &nbsp; <a href="javascript:void(0);" class="remCF">Remove</a></td></tr>');
        $("#customFields").append('<tr valign="top"><td><input id="myText" style="width:100%" class="code" /></td><td><input type="text" id="myText2" /> &nbsp; <a href="javascript:void(0);" class="remCF">Remove</a></td></tr>');
        
        $(".code").autocomplete({
            source: dataSrc
        });
    });
    $(".code").autocomplete({
        source: dataSrc
    });
        $("#customFields").on('click', '.remCF', function() {
            $(this).parent().parent().remove();
        });


    });
</script>

<table class="form-table" id="customFields">
<tr valign="top">
    <td>
        <input id="myText1" style="width:100%" class="code" /> &nbsp;
        </td>
        <td>
        <input type="text" id="myText2" /> &nbsp;
        <a href="javascript:void(0);" id="addCF" class="addCF">Add</a>
    </td>
</tr>
</table>
</td>
</tr>--%>
        <tr>
            <td>

                <script>


                    var dataSrc = ["Agathi Keerai அகத்தி கீரை 559", "Abutilon indicum துத்திக் கீரை 640", "Amaranth முளைக்கீரை 340", "Amaranthus-Organic அரைகீரை 479", "Amaranthus Tender தண்டுகீரை 275", "Amaranthus Tricolor அரைகீரை 232", "Ballon Vines-Destem முடக்கத்தான்கீரை 300", "Black Nightshade மணத்தக்காளிகீரை 337", "Black Nightshade-seed மணத்தக்காளிக்காய் 641", "Drumstick Leaves முருங்கை கீரை 569", "Green Amaranthus Tender-Organic தண்டுகீரை 586", "Henna Leaf மருதாணி 638", "Hibiscus Flower செம்பருத்தி 643", "Hibiscus Leaf செம்பருத்தி 644", "Indian Pennywort-Destem வல்லாரை 304", "Keela Nelli கீழா நெல்லி 565", "Kuppaimeni குப்பைமேனி 568", "Lambsquarters சக்கரவர்த்தி கீரை 639", "Paruppu keerai பருப்பு கீரை 558", "Purple Lippia பொட்டுத்தலை 637", "Ponnanganni Keerai பொன்னாங்கண்ணிகீரை 425", "White Karisalankanni வெள்ளை கரிசலாங்கண்ணி கீரை 554", "Red Amaranth சிவப்பு முளைக்கீரை 556", "Red Amaranthus Tender-Organic தண்டுகீரை 449", "ThaiNightShade-Destem தூதுவளை 303", "Radish-Organic முள்ளங்கி 454", "Red Ponnanganni Keerai சிவப்பு பொன்னாங்கண்ணி கீரை 557", "Silver cocks comb பண்ணை கீரை 642", "Sorrel Leaves-Organic புளிச்சகீரை 514", "Yellow Karisalankanni மஞ்சள் கரிசலாங்கண்ணி கீரை 563", "Sorrel Leaves புளிச்சகீரை 341", "Tanners Cassia ஆவாரம்பூ 561", "Tropical Amaranth-Organic சிறுகீரை 448", "Tropical Amaranth சிறுகீரை 339", "Vadhanarayana keerai வாதநாராயண கீரை 566", "Veldt Grape பிரண்டை 349", "Vendhaya Keerai வெந்தயக் கீரை 444", "Spinach green பசலைகீரை 301", "Spring onion வெங்காயத்தாள் 302", "Sukkan Keerai சுக்கான் கீரை 560", "Palak பாலக்கீரை 265", "Musumusukkai Keerai முசுமுசுக்கை கீரை 567", "Agathi Keerai-Organic அகத்தி கீரை 662", "Amla-Organic நெல்லி 655", "Banana Karpuram-Organic கற்பூரவாழை 610", "Banana Pith-Organic வாழைத்தண்டு 664", "Banana Rasthali-Organic ரஸ்தாலி 612", "Banana Red-Organic செவ்வாழை 611", "Banana Poovan-Organic பூவன்வாழை  650", "Bottle Gourd-Organic சுரக்காய் 505", "Brinjal Ujala-Organic உஜாலாகத்தரி 691", "Brinjal Vari-Organic வரிகத்தரி 543", "Brinjal Tube Purple-Organic டியூப்கத்தரி 614", "Brinjal Tube Green-Organic டியூப்கத்தரி 615", "Bush Beans-Organic பீன்ஸ் 453", "Carrot Purple-Organic கேரட் 518", "Carrot Red-Organic கேரட் 519", "Carrot White-Organic கேரட் 520", "Coriander-Organic கொத்தமல்லி 452", "Drumstick-Organic முருங்கை 592", "Greenchilli-Organic மிளகாய் 455", "Guava-Organic கொய்யா 661", "Ivy Gourd-Organic கோவக்காய் 571", "Kale-Organic 504", "Ladiesfinger-Organic வெண்டைக்காய் 456", "Palak-Organic பாலக்கீரை 451", "Papaya Red Lady-Organic பப்பாளி 616", "Ponnanganni Keerai-Organic பொன்னாங்கண்ணிகீரை 679", "Swiss Chard-Organic 469", "Watermelon-Organic தர்பூசணி 680", "WheatGrass கோதுமைபுல் 410", "Yellow Pumpkin-Organic மஞ்சள்பூசணி 447", "Asparagus அஸ்பாரகஸ் 443", "Amla நெல்லி 234", "Ash Gourd வெள்ளைபூசணி 280", "Baby Corn-Raw இளஞ்சோளம் 297", "Banana Flower வாழைப்பூ 284", "Beetroot பீட்ருட் 236", "Big Onion பெரியவெங்காயம் 237", "Big Onion Grade B பெரியவெங்காயம் 594", "Bitter Gourd பாகற்காய் 266", "Baby Bitter Gourd சிறியபாகற்காய் 613", "Bottle Gourd சுரக்காய் 273", "Brinjal Disco டிஸ்கோகத்தரி 358", "Brinjal Palayam பாளையம்கத்தரி 359", "Brinjal Subal சுபால்கத்தரி 240", "Brinjal Tube டியூப்கத்தரி 360", "Brinjal Ujala உஜாலாகத்தரி 239", "Brinjal Vari வரிகத்தரி 238", "Brinjal-White வெள்ளைகத்தரி 685", "Broad Beans அவரை 233", "Broad Beans-Country நாட்டுஅவரை 363", "Butter Beans பட்டர்பீன்ஸ் 286", "Cabbage முட்டைக்கோஸ் 242", "Capsicum Green பச்சைகுடைமிளகாய் 243", "Capsicum Red சிவப்புகுடைமிளகாய் 244", "Capsicum Yellow மஞ்சள்குடைமிளகாய் 245", "Carrot கேரட் 246", "Carrot Delhi கேரட் 656", "Cauliflower காலிஃபிளவர் 696", "Chilli Bajji பஜ்ஜிமிளகாய் 283", "Chowchow சவ்சவ் 272", "Cluster Beans கொத்தவரக்காய் 255", "Coconut-Organic தேங்காய் 484", "Coconut தேங்காய் 247", "Coconut medium தேங்காய் 657", "Chinese potato சிறுகிழங்கு 431", "Cucumber வெள்ளரி 250", "Dosakai தோசை காய் 528", "Double Beans டபுள்பீன்ஸ் 435", "Double Beans-Peeled டபுள்பீன்ஸ் 660", "Drumstick முருங்கை 262", "Field beans மொச்சை 321", "Garlic Country நாட்டுப்பூண்டு 287", "Garlic Malai மலைப்பூண்டு 285", "Garlic Uthiri உதிரிபூண்டு 288", "Garlic Kodai Grade A பூண்டு 665", "Garlic Kodai Grade B பூண்டு 666", "Garlic Solo Kodaikanal பூண்டு 667", "Garlic Himachal பூண்டு 668", "Garlic Himachal-splittedபூண்டு 669", "Ginger Fresh இஞ்சி 365", "Ginger இஞ்சி 252", "Green Peas பச்சைபட்டாணி 409", "Greenchilli மிளகாய் 253", "Greenchilli-Country மிளகாய் 688", "Ground nut நிலக்கடலை 254", "Ivy Gourd கோவக்காய் 289", "Raw Jack Fruit பலாக்காய் 291", "Kankoda கன்கோடா 256", "Turnip நூல்கோல் 264", "Ladiesfinger வெண்டைக்காய் 257", "Lemon small சிறியஎலுமிச்சை 258", "Lemon-Big பெரியஎலுமிச்சை 370", "Long Beans காராமணி 290", "Lotus Stem 593", "Lucky Tiger-Tomato 683", "Malabar Cucumber மலபார் வெள்ளரிக்காய் 532", "Manila Tamarind கொடுக்காப்புளி 494", "Ooty Cucumber ஊட்டிவெள்ளரி 344", "Parwal பர்வல் 269", "Pigeon Pea துவரை 408", "Pidi Karunai கருணைக்கிழங்கு 598", "Pinju vellari பிஞ்சு வெள்ளரி 446", "Potato உருளைக்கிழங்கு 267", "Potato Agra உருளைக்கிழங்கு 652", "Panang Kizhangu பனங்கிழங்கு 424", "Mango ginger இஞ்சி 595", "Radish Red முள்ளங்கி 529", "Radish முள்ளங்கி 261", "Raw Banana வாழைக்காய் 278", "Raw Mango மாங்காய் 292", "Red Cabbage சிவப்பு முட்டைக்கோஸ் 342", "Red Turnip நூல்கோல் 609", "Ridge Gourd பீர்கங்காய் 407", "Ring Beans பீன்ஸ் 235", "Small Bitter Gourd சிறியபாகற்காய் 259", "Small Onion சிறியவெங்காயம் 271", "Snake Gourd புடலங்காய் 268", "Sweet Corn மக்காச்சோளம் 274", "Sweet Potato சர்க்கரைவள்ளிகிழங்கு 294", "Tamarind புளி 295", "Tapioca மரவள்ளி 361", "Taro Root சேப்பங்கிழங்கு 270", "Tender mango வடு மாங்காய் 477", "Tinda தின்டா 687", "Tomato-Bangalore பெங்களூர்தக்காளி 276", "Tomato Country-Organic நாட்டுதக்காளி 544", "Tomato-Country நாட்டுதக்காளி 277", "Tomato-Green பச்சை தக்காளி 686", "Turkey Berry சுண்டைக்காய் 350", "Turnip-Organic நூல்கோல் 513", "Unripe Figs அத்தி காய் 564", "White Beans பீன்ஸ் 372", "White Onion வெள்ளை வெங்காயம் 553", "Yam சேனைக்கிழங்கு 296", "Yellow Pumpkin மஞ்சள்பூசணி 281", "Apple Washington ஆப்பிள் 307", "Apple Ambri ஆப்பிள் 602", "Apple Golden ஆப்பிள் 599", "Apple Fuji ஆப்பிள் 369", "Apple Royal Gala ஆப்பிள் 305", "Apple Shimla ஆப்பிள் 517", "Avocado வெண்ணெய் பழம் 464", "Avocado Imported வெண்ணெய் பழம் 465", "Banana Country நாட்டுவாழை 689", "Banana Green பச்சை வாழை 522", "Banana Hills மலை வாழை 503", "Banana karpuram கற்பூரவாழை 423", "Banana Nendran நேந்திரவாழை 422", "Banana poovan பூவன்வாழை  427", "Banana Rasthali ரஸ்தாலி 581", "Banana RED செவ்வாழை 309", "Banana Yelaki ஏலக்கி 502", "Banana Morris மோரிஸ்வாழை 308", "Ber Fruit இலந்தை பழம் 654", "Custard Apple சீத்தாப்பழம் 618", "Citroen நாரத்தங்காய் 636", "Grapes Juice திராட்சை 310", "Grapes Green Seedless பச்சை திராட்சை  430", "Grapes Black Seedless கருப்பு திராட்சை 429", "Grapes Paneer திராட்சை 605", "Grapes Red Globe சிவப்பு திராட்சை 604", "Green Apple USA ஆப்பிள் 457", "Guava கொய்யா 311", "Guava pink கொய்யா 603", "Grapefruit பப்ளிமாசு 633", "Indian Lychee லிச்சி 694", "JackFruit பலாப்பழம் 506", "Jamun நாகப்பழம் 574", "Longan கடுகுடாப் பழம் 601", "Kiwi கிவி 441", "Mulampalam முலாம்பழம் 527", "Muskmelon கிர்ணிபழம் 319", "Orange Imported ஆரஞ்சு 524", "Orange Kodai ஆரஞ்சு 572", "Orange Malta ஆரஞ்சு 440", "Orange Nagpur ஆரஞ்சு 478", "Orange Kamala ஆரஞ்சு 314", "Orange Kinnow ஆரஞ்சு 653", "Original Kimia Dates கிமியா பேரிச்சம்பழம் 357", "Papaya பப்பாளி 320", "Passion Fruits தாட்பூட்பழம் 495", "Pears Green பச்சை பேரிக்காய் 583", "Pears ooty ஊட்டிபேரிக்காய் 607", "Pears Golden பேரிக்காய் 606", "Pears Red சிவப்பு பேரிக்காய் 582", "Pears பேரிக்காய் 315", "Pineapple அன்னாசி 345", "Pomegranate மாதுளை 317", "Pomegranate-Country மாதுளை 684", "Sapota சப்போட்டா 421", "Sun melon சன் முலாம்பழம் 521", "Sweet Lime சாத்துக்குடி 371", "Watermelon Hybrid தர்பூசணி 346", "Watermelon தர்பூசணி 438", "Watermelon Yellow தர்பூசணி 681", "Water Rose Apple 682", "Cherries செர்ரி 460", "Blueberry அவுரிநெல்லி 459", "Dragon Fruit தறுகண்பழம் 463", "Durian Fruit முள்நாரிப்பழம் 634", "Apricot-Exotic 1Kg சர்க்கரை பாதாமி 549", "Cherry-Exotic 1kg சேலாப்பழம் 547", "Fresh Dates-Exotic 1Kg பேரிச்சம்பழம் 551", "Exotic-Mangosteen மங்குஸ்தான் 313", "Peach-Exotic 1kg குழிப்பேரி 548", "Exotic-Rambutan 1kg ரம்புட்டான் 546", "Fresh Fig அத்திப்பழம் 523", "Plums Imported ஆல்பக்கோடா 466", "Plums Kodaikanal ஆல்பக்கோடா 552", "Kaki Persimmon சீமைப் பனிச்சை 635", "Red Currant செங்கொடிமுந்திரி 458", "Starfruit விளிம்பிப்பழம் 573", "Strawberry செம்புற்றுப்பழம் 461", "Sweet Tamarind இனிப்பு புளி 580", "Thai Guava கொய்யா 462", "Yellow dates மஞ்சள் பேரிச்சம்பழம் 579", "Banana Pith வாழைத்தண்டு 279", "Beetle Leaf வெற்றிலை 355", "Coriander கொத்தமல்லி 248", "Curryleaves கறிவேப்பிலை 251", "Edu Ilai வாழைஇலை 356", "Mint புதினா 260", "Thala Ilai தலைவாழை இலை 354", "Broccoli ப்ரோக்கோலி 241", "Brussels sprouts களைக்கோசு 555", "Basil பசில் 467", "Celery செலரி 335", "Chineese Cabbage சீனகோஸ் 334", "Dill சதகுப்பி 631", "Leeks லீக்ஸ் 336", "Lemon Grass 507", "Lettuce இலைக்கோஸ் 428", "Lettuce Green இலைக்கோஸ் 617", "Lettuce Purpleஇலைக்கோஸ் 480", "Parsley வோக்கோசு 623", "Bok Choy போக் சோய் 693", "Tomato -Cherry செர்ரி தக்காளி 508", "Yellow Zuccini மஞ்சள்சுச்சினி 368", "Zuccini சுச்சினி 343", "Button Mushroom காளான் 406", "Baby Corn-Peeled இளஞ்சோளம் 298", "Baby Potato சிறியஉருளை 299", "Black chickpeas Sprouts முளைகட்டிய கொண்டைக் கடலை 472", "Cowpea Sprouts முளைகட்டிய காராமணி 473", "Green Gram Sprouts முளைகட்டிய பச்சைபயிறு 475", "Horse Gram Sprouts முளைகட்டிய கொள்ளு 471", "Mixed Sprouts கலப்பு முளைகட்டிய பயறு 474", "Pearl Millet Sprouts முளைகட்டிய கம்பு 476", "Sorghum Sprouts முளைகட்டிய சோளம் 690", "Rumani Mango 1 Kg 578", "Rumani Mango 2.5 Kg 576", "Banganapalli Mango 2.5 Kgs 695", "Banganapalli Mango 1 Kg 545", "Red Kidney Beans பீன்ஸ் 699", "RamSita சீத்தாப்பழம் 701", "Potato Mettupalayam உருளைக்கிழங்கு 705", "Coconut - Suruli Falls தேங்காய் 725", "Kiwi Golden கிவி 729", "Insulin Keerai இன்சுலின் கீரை 663", "Banana Saba பேய் வாழை 627", "Rosemary ரோஸ்மேரி 773", "Aloe Vera One Leaf அலோ வேரா  282"];
                    
                    function getproducts() {
                        var dataSrc1;
                        var datasample;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "ffhpservice.asmx/getproducts",
                            data: "{'name':'test'}",
                            dataType: "json",
                            success: function(data) {
                                var data2 = $.parseJSON(data.d);
                                for (var i = 0; i < data2.length; i++) {
                                    if (datasample == "") {
                                        if (data2[i].length > 0) {
                                            datasample = data2[i];
                                        }
                                    }
                                    else {
                                        if (data2[i].length > 0) {
                                            datasample = datasample + "\",\"" + data2[i];
                                        }
                                    }

                                }
                                dataSrc1 = "[\"" + datasample + "\"]";
                            },
                            error: function(result) {
                                alert("Error");
                            }
                        });
                        alert(dataSrc1);
                        return dataSrc;
                    }

                    function validate() {
                        var re = true;
                        $(".code,.proweight,.propc,.protryweight").each(function() {
                            if ($.trim($(this).val()) == "") {
                                $(this).focus();
                                re = false;
                                //return false;
                            }

                        });
                        return re;

                    }
                    $(document).ready(function() {
                    
                    
                        

                        $("#get").show();
                        $("#submit").hide();
                        $("#customFields").hide();
                        $(".addCF").click(function() {
                            var rowCount = $("#customFields tr").length;
                            //        $("#customFields").append('<tr valign="top"><th scope="row"><label for="customFieldName">Custom Field</label></th><td><input id="myText" style="width:100%" class="code" /> &nbsp; <input type="text" id="myText2" /> &nbsp; <a href="javascript:void(0);" class="remCF">Remove</a></td></tr>');
                            //<td style="background-color:#CEE7F7;color:#212463;"><input id="txtproductid' + rowCount + '" class="productid" style="width:50%" /></td>
                            $("#customFields").append('<tr><td style="background-color:#CEE7F7;color:#212463;text-align:center;"><input id="myText' + rowCount + '" class="code" style="width:99%" /></td><td style="background-color:#CEE7F7;color:#212463;text-align:center;"><input id="txtmorningscannedweight' + rowCount + '" value="0" class="proweight" style="width:90%" /></td><td style="background-color:#CEE7F7;color:#212463;text-align:center;"><input id="txtmorningpiececount' + rowCount + '" value="0" value="0" class="propc" style="width:90%" /></td><td style="background-color:#CEE7F7;color:#212463;text-align:center;"><input id="txtmorningtrayweight' + rowCount + '" value="0" class="protryweight" style="width:90%"/></td><td style="background-color:#CEE7F7;color:#212463;text-align:center;"><input id="txtmorningdescription' + rowCount + '" class="prodescription" style="width:90%"/><a href="javascript:void(0);" class="remCF">Remove</a></td></tr>');

                            $(".code").autocomplete({
                            source: dataSrc
                            });
                            $(".remCF").hide();
                            $(".remCF").last().show();



                            $(".prodescription").off("keydown");


                            $(".prodescription").last().keydown(function(e) {
                                if (e.which == 9)
                                    $(".addCF").trigger('click');

                                //$(this).on("keydown");

                            });



                        });

                        $(".code").autocomplete({
                        source: dataSrc
                        });

                        $("#customFields").on('click', '.remCF', function() {
                            $(this).parent().parent().remove();
                            $(".remCF").hide();
                            $(".remCF").last().show();
                        });

                        $(".prodescription").keydown(function(e) {
                            if (e.which == 9)
                                $(".addCF").trigger('click');

                            //$(".prodescription").off("keydown");
                            //$(".prodescription").last().on("keydown");
                        });



                        $("#submit").click(function() {
                            //alert("Hi");
                            if (validate()) {
                                var Name = null;
                                var proweight = null;
                                var propc = null;
                                var rowjsonstringvalue = null;
                                var jsonstringvalue = null;
                                //alert($("#<%= ddlstocktype.ClientID %>").val());
                                $("#customFields").find('tr:gt(0)').each(function() {
                                    var rowindx = $(this).index();
                                    //alert(rowindx);
                                    if (Name == null) {
                                        Name = $("#myText" + rowindx).val();
                                    }
                                    else {
                                        Name = Name + ',' + $("#myText" + rowindx).val();
                                    }
                                    if (proweight == null) {
                                        proweight = $("#txtmorningscannedweight" + rowindx).val();
                                    }
                                    else {
                                        proweight = proweight + ',' + $("#txtmorningscannedweight" + rowindx).val();
                                    }
                                    if (propc == null) {
                                        propc = $("#txtmorningpiececount" + rowindx).val();
                                    }
                                    else {
                                        propc = propc + ',' + $("#txtmorningpiececount" + rowindx).val();
                                    }
                                    //alert(Name);
                                    //alert(proweight);
                                    //alert(propc);
                                    rowjsonstringvalue = "{" + "\"productid\"" + ":\"" + $("#myText" + rowindx).val().slice(-3) + "\"," + "\"Name\"" + ":\"" + $("#myText" + rowindx).val() + "\"," + "\"morningscannedweight\"" + ":\"" + $("#txtmorningscannedweight" + rowindx).val() + "\"," + "\"morningpiececount\"" + ":\"" + $("#txtmorningpiececount" + rowindx).val() + "\"," + "\"morningtrayweight\"" + ":\"" + $("#txtmorningtrayweight" + rowindx).val() + "\"," + "\"morningdescription\"" + ":\"" + $("#txtmorningdescription" + rowindx).val() + "\"}";
                                    if (jsonstringvalue == null) {
                                        jsonstringvalue = rowjsonstringvalue;
                                    }
                                    else {
                                        jsonstringvalue = jsonstringvalue + "," + rowjsonstringvalue;
                                    }

                                    //alert(jsonstringvalue);

                                });

                                $.ajax({

                                    type: 'POST',

                                    url: 'ffhpservice.asmx/Update_Stock_Status',

                                    data: "{'jsonstring':'" + "[" + jsonstringvalue + "]" + "','stocktype':'" + $("#<%= ddlstocktype.ClientID %>").val() + "'}",

                                    contentType: "application/json; charset=utf-8",

                                    dataType: "json",

                                    success: function(r) {
                                        alert(r.d);
                                    },
                                    error: function(r) {
                                        alert(r.responseText);
                                    },
                                    failure: function(r) {
                                        alert(r.responseText);
                                    }

                                });

                            }
                        });
                        //begin - get stock status
                        $("#get").click(function() {
                            if ($("#<%= ddlstocktype.ClientID %>").val() != "0") {
                                $("#submit").show();
                                $("#customFields").show();
                                $.ajax({
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    url: "ffhpservice.asmx/Get_Stock_Status",
                                    data: "{'stocktype':'" + $("#<%= ddlstocktype.ClientID %>").val() + "'}",
                                    dataType: "json",
                                    success: function(data) {
                                        var data2 = $.parseJSON(data.d);
                                        for (var i = 0; i < data2.length; i++) {
                                            //alert(data2[i].productid);
                                            if (i == 0) {

                                                $("#myText1").val(data2[i].name + " " + data2[i].productid);
                                                $("#txtmorningscannedweight1").val(data2[i].morningscannedweight);
                                                $("#txtmorningpiececount1").val(data2[i].morningpiececount);
                                                $("#txtmorningtrayweight1").val(data2[i].morningtrayweight);
                                            }
                                            else {
                                                var rowCount = $("#customFields tr").length;
                                                $("#customFields").append('<tr><td style="background-color:#CEE7F7;color:#212463;text-align:center;"><input id="myText' + rowCount + '" value="' + data2[i].name + " " + data2[i].productid + '" class="code" style="width:99%" /></td><td style="background-color:#CEE7F7;color:#212463;text-align:center;"><input id="txtmorningscannedweight' + rowCount + '" value="' + data2[i].morningscannedweight + '" class="proweight" style="width:90%" /></td><td style="background-color:#CEE7F7;color:#212463;text-align:center;"><input id="txtmorningpiececount' + rowCount + '" value="' + data2[i].morningpiececount + '" class="propc" style="width:90%" /></td><td style="background-color:#CEE7F7;color:#212463;text-align:center;"><input id="txtmorningtrayweight' + rowCount + '" value="' + data2[i].morningtrayweight + '" class="protryweight" style="width:90%"/></td><td style="background-color:#CEE7F7;color:#212463;text-align:center;"><input id="txtmorningdescription' + rowCount + '" value="' + data2[i].morningdescription + '" class="prodescription" style="width:90%"/><a href="javascript:void(0);" class="remCF">Remove</a></td></tr>');

                                                $(".code").autocomplete({
                                                source: dataSrc
                                                });
                                                $(".remCF").hide();
                                                $(".remCF").last().show();



                                                $(".prodescription").off("keydown");


                                                $(".prodescription").last().keydown(function(e) {
                                                    if (e.which == 9)
                                                        $(".addCF").trigger('click');

                                                    //$(this).on("keydown");

                                                });
                                            }
                                        }
                                    },
                                    error: function(result) {
                                        alert("Error");
                                    }
                                });
                                $("#get").hide();
                            }
                        });
                        //end  -  get stock status

                    });

    
                </script>

                <asp:Repeater ID="Repeater1" runat="server">
                    <HeaderTemplate>
                        <table class="form-table" width="100%" id="customFields" cellspacing="0" rules="all"
                            border="1">
                            <tr bgcolor="#0071BD" style="background-color: #0071BD; font-weight: bold; color: White;">
                                <%--<th scope="col">
                    Product Id
                </th>--%>
                                <th scope="col" width="50%">
                                    Name <a href="javascript:void(0);" id="addCF" class="addCF" style="color: WhiteSmoke;">
                                        Add New</a>
                                </th>
                                <th scope="col" width="10%">
                                    Weight
                                </th>
                                <th scope="col" width="10%">
                                    Pc Count
                                </th>
                                <th scope="col" width="10%">
                                    Tray Weight
                                </th>
                                <th scope="col" width="10%">
                                    Description
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <%--<td style="background-color:#CEE7F7;color:#212463;">
                <input id="txtproductid1" class="productid" style="width:50%" />
            </td>--%>
                            <td style="background-color: #CEE7F7; color: #212463; text-align: center;">
                                <input id="myText1" class="code" style="width: 99%" />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463; text-align: center;">
                                <input id="txtmorningscannedweight1" value="0" class="proweight" style="width: 90%;" />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463; text-align: center;">
                                <input id="txtmorningpiececount1" value="0" class="propc" style="width: 90%;" />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463; text-align: center;">
                                <input id="txtmorningtrayweight1" value="0" class="protryweight" style="width: 90%;" />
                            </td>
                            <td style="background-color: #CEE7F7; color: #212463; text-align: center;">
                                <input id="txtmorningdescription1" class="prodescription" style="width: 90%;" />
                            </td>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tr> </table>
                    </FooterTemplate>
                </asp:Repeater>
            </td>
        </tr>
        <tr>
            <td>
                <a href="javascript:void(0);" id="submit">Submit</a>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btndownload" Text="Download Weight Difference Report" runat="server"
                                OnClick="btndownload_OnClick" OnClientClick="return btndownload_Click();" />
                        </td>
                        <td>
                            <asp:Button ID="btndataupload" Text="Data Upload" runat="server" OnClick="btndataupload_OnClick" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
