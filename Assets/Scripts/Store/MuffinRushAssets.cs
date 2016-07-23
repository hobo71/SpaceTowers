using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store.Example {

	/// <summary>
	/// This class defines our game's economy, which includes virtual goods, virtual currencies
	/// and currency packs, virtual categories
	/// </summary>
	public class MuffinRushAssets : IStoreAssets{

	
		public int GetVersion() {
			return 0;
		}

		public VirtualCurrency[] GetCurrencies() {
			return new VirtualCurrency[]{MUFFIN_CURRENCY};
		}

	    public VirtualGood[] GetGoods() {
			return new VirtualGood[] {};
		}
			
	    public VirtualCurrencyPack[] GetCurrencyPacks() {
			return new VirtualCurrencyPack[] {Pack1,Pack2,Pack3,Pack4,Pack5,Pack6,Pack7,Pack8};
		}

	    public VirtualCategory[] GetCategories() {
			return new VirtualCategory[]{};
		}

	    /** Static Final Members **/

	    public const string MUFFIN_CURRENCY_ITEM_ID      = "currency_muffin";

		public const string PRODUCT_ID1 = "com.spacetower.credit2";
		public const string PRODUCT_ID2 = "com.spacetower.credit4";
		public const string PRODUCT_ID3 = "com.spacetower.credit10";
		public const string PRODUCT_ID4 = "com.spacetower.credit20";
		public const string PRODUCT_ID5 = "com.spacetower.credit40free2";
		public const string PRODUCT_ID6 = "com.spacetower.credit100free6";
		public const string PRODUCT_ID7 = "com.spacetower.credit200free10";
		public const string PRODUCT_ID8 = "com.spacetower.credit300free16";
		public const string PRODUCT_ID9 = "com.spacetower.credit500free40";

	    /** Virtual Currencies **/

	    public static VirtualCurrency MUFFIN_CURRENCY = new VirtualCurrency(
	            "Muffins",										// name
	            "",												// description
	            MUFFIN_CURRENCY_ITEM_ID							// item id
	    );


	    /** Virtual Currency Packs **/

		public static  VirtualCurrencyPack Pack1 = new VirtualCurrencyPack(
			"2 credit",                                   // name
			"2 credit",                     // description
			"2 credit_item_id",                            // item id
			2,											// number of currencies in the pack
			MUFFIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
			new PurchaseWithMarket(PRODUCT_ID1, 1)
		);

		public static  VirtualCurrencyPack Pack2= new VirtualCurrencyPack(
			"4 credit",                                   // name
			"4 credit",                     // description
			"4 credit_item_id",                            // item id
			4,		                             // number of currency units in this pack
			MUFFIN_CURRENCY_ITEM_ID,                   // the currency associated with this pack
			new PurchaseWithMarket(PRODUCT_ID2,2)    // purchase type
		);
		public static  VirtualCurrencyPack Pack3 = new VirtualCurrencyPack(
			"10 credit",                                   // name
			"10 credit",                     // description
			"10 credit_item_id",                            // item id
			10,		                                       // number of currency units in this pack
			MUFFIN_CURRENCY_ITEM_ID,                   // the currency associated with this pack
			new PurchaseWithMarket(PRODUCT_ID3, 5)    // purchase type
		);
		public static  VirtualCurrencyPack Pack4 = new VirtualCurrencyPack(
			"20 credit",                                   // name
			"20 credit",                     // description
			"20 credit_item_id",                            // item id
			20,		                         // number of currency units in this pack
			MUFFIN_CURRENCY_ITEM_ID,                   // the currency associated with this pack
			new PurchaseWithMarket(PRODUCT_ID4,10)    // purchase type
		);
		public static  VirtualCurrencyPack Pack5 = new VirtualCurrencyPack(
			"40+2 credit",                                   // name
			"40+2 credit",                     // description
			"40+2 credit_item_id",                            // item id
			42,		                                     // number of currency units in this pack
			MUFFIN_CURRENCY_ITEM_ID,                   // the currency associated with this pack
			new PurchaseWithMarket(PRODUCT_ID5, 20)    // purchase type
		);
		public static  VirtualCurrencyPack Pack6 = new VirtualCurrencyPack(
			"100+6 credit",                                   // name
			"100+6 credit",                     // description
			"100+6 credit_item_id",                            // item id
			106,		                                      // number of currency units in this pack
			MUFFIN_CURRENCY_ITEM_ID,                   // the currency associated with this pack
			new PurchaseWithMarket(PRODUCT_ID6,50)    // purchase type
		);
		public static  VirtualCurrencyPack Pack7 = new VirtualCurrencyPack(
			"200+10 credit",                                   // name
			"200+10 credit",                     // description
			"200+10 credit_item_id",                            // item id
			210,		                                     // number of currency units in this pack
			MUFFIN_CURRENCY_ITEM_ID,                   // the currency associated with this pack
			new PurchaseWithMarket(PRODUCT_ID7, 100)    // purchase type
		);
		public static  VirtualCurrencyPack Pack8 = new VirtualCurrencyPack(
			"300+16 credit",                                   // name
			"300+16 credit",                     // description
			"300+16 credit_item_id",                            // item id
			316,		                               // number of currency units in this pack
			MUFFIN_CURRENCY_ITEM_ID,                   // the currency associated with this pack
			new PurchaseWithMarket(PRODUCT_ID8, 150)    // purchase type
		);
		public static  VirtualCurrencyPack Pack9 = new VirtualCurrencyPack(
			"500+40 credit",                                   // name
			"500+40 credit",                     // description
			"500+40 credit_item_id",                            // item id
			540,		                                // number of currency units in this pack
			MUFFIN_CURRENCY_ITEM_ID,                   // the currency associated with this pack
			new PurchaseWithMarket(PRODUCT_ID9, 250)    // purchase type
		);




	}

}
