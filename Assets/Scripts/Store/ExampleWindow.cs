
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Soomla;

namespace Soomla.Store.Example {

	public class ExampleWindow : MonoBehaviour {

		private static ExampleWindow instance = null;

		public UILabel BabyBundleLabel;
		public UILabel ExtraLargeLabel;
		public UILabel BrettHeartLabel;
		public UILabel CasualtyBundleLabel;
		public UILabel KevinBundleLabel;
		public UILabel BigmanBundleLabel;
		public UILabel SmallBundleLabel;
		public UILabel DoubleBundleLabel;

		private bool checkAffordable = false;

		public UILabel IAPResultLabel;

		void Awake(){
			if(instance == null){ 	//making sure we only initialize one instance.
				instance = this;
				GameObject.DontDestroyOnLoad(this.gameObject);
			} else {					//Destroying unused instances.
				GameObject.Destroy(this);
			}

		}

		private Dictionary<string, Texture2D> itemsTextures;
		private Dictionary<string, bool> itemsAffordability;

        private Reward firstLaunchReward;


		void Start () {

//			IStoreSetup ();

			StoreEvents.OnSoomlaStoreInitialized += onSoomlaStoreInitialized;
			StoreEvents.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
			StoreEvents.OnUnexpectedStoreError += onUnexpectedStoreError;
			StoreEvents.OnItemPurchased += onItemPurchased;


			SoomlaStore.Initialize(new MuffinRushAssets());
		}


		void IStoreSetup(){

			BabyBundleLabel.text = "£1.49";
			ExtraLargeLabel.text = "£79.99";
			BrettHeartLabel.text = "£35.99";
			CasualtyBundleLabel.text = "£6.99";
			KevinBundleLabel.text = "£2.99";
			BigmanBundleLabel.text = "£37.99";
			SmallBundleLabel.text = "£7.99";
			DoubleBundleLabel.text = "£2.29";

		}


		public void onUnexpectedStoreError(int errorCode) {
			SoomlaUtils.LogError ("ExampleEventHandler", "error with code: " + errorCode);
		}

		public void onSoomlaStoreInitialized() {

			if (StoreInfo.Currencies.Count>0) {
				try {
//					//First launch reward
//                    if(!firstLaunchReward.Owned)
//                    {
//                        firstLaunchReward.Give();
//                    }

				} catch (VirtualItemNotFoundException ex){
					SoomlaUtils.LogError("SOOMLA ExampleWindow", ex.Message);
				}
			}

			setupItemsTextures();

			setupItemsAffordability ();
		}

		public void setupItemsTextures() {
			itemsTextures = new Dictionary<string, Texture2D>();

			foreach(VirtualCurrencyPack vcp in StoreInfo.CurrencyPacks){
				itemsTextures[vcp.ItemId] = (Texture2D)Resources.Load("SoomlaStore/images/" + vcp.Name);
			}
		}

		public void setupItemsAffordability() {
				itemsAffordability = new Dictionary<string, bool> ();
		}

		public void onCurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded) {
			if (itemsAffordability != null)
			{
				List<string> keys = new List<string> (itemsAffordability.Keys);
				foreach(string key in keys)
					itemsAffordability[key] = StoreInventory.CanAfford(key);
			}
		}

		public void onItemPurchased(PurchasableVirtualItem pvi, string payload) {

			//IAPResultLabel.text = pvi.Name + " Purchased";
			print (">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
			print ("PurchasePayload: " + payload);
			print ("ItemPurchased: " + pvi.ItemId + " Name: " + pvi.Name);
			print (">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

			if (pvi.ItemId.Equals("babybundle_item_id")) 
			{
	//			PlayerPrefs.SetInt(Constants.CurrentCookies,PlayerPrefs.GetInt(Constants.CurrentCookies)+4000);

			}
			if (pvi.ItemId.Equals("extralargebundle_item_id")) 
			{
	//			PlayerPrefs.SetInt(Constants.CurrentCookies,PlayerPrefs.GetInt(Constants.CurrentCookies)+200000);
	//			PlayerPrefs.SetInt(Constants.CurrentLives,PlayerPrefs.GetInt(Constants.CurrentLives)+1000);
			}
			if (pvi.ItemId.Equals("bretthart_item_id")) 
			{
		//		PlayerPrefs.SetInt(Constants.CurrentLives,PlayerPrefs.GetInt(Constants.CurrentLives)+300);
			}

			if (pvi.ItemId.Equals("casultybundle_item_id")) 
			{
		//		PlayerPrefs.SetInt(Constants.CurrentLives,PlayerPrefs.GetInt(Constants.CurrentLives)+50);
			}
			if (pvi.ItemId.Equals("kevinhartbundle_item_id")) 
			{
		//		PlayerPrefs.SetInt(Constants.CurrentLives,PlayerPrefs.GetInt(Constants.CurrentLives)+30);
			}
			if (pvi.ItemId.Equals("bigmanbundle_item_id")) 
			{
	//			PlayerPrefs.SetInt(Constants.CurrentCookies,PlayerPrefs.GetInt(Constants.CurrentCookies)+150000);
			}
			if (pvi.ItemId.Equals("smallmanbundle_item_id")) 
			{
	//			PlayerPrefs.SetInt(Constants.CurrentCookies,PlayerPrefs.GetInt(Constants.CurrentCookies)+25000);
			}
			if (pvi.ItemId.Equals("doublecookiesbooster_item_id")) 
			{
	//			PlayerPrefs.SetInt(Constants.DoubleCookiesBooster,22);

			}

		//	if (CamManager.isInternetConnected() && Constants.IsFbUser) {
	//			UpdateStorage.UpdateData();
	//		}


		}

		void Update () {
			

		}

			
		public void Pack1() {
			try {
				Debug.Log("attempt to purchase");

				StoreInventory.BuyItem("babybundle_item_id"); 
														// if the purchases can be completed sucessfully
			} 
			catch (Exception e) 
			{																						// if the purchase cannot be completed trigger an error message connectivity issue, IAP doesnt exist on ItunesConnect, etc...)
				Debug.Log ("SOOMLA/UNITY" + e.Message);							
			}

		}
		public void Pack2(){
			try {
				Debug.Log("attempt to purchase");

				try { 
					StoreInventory.BuyItem("extralargebundle_item_id"); 
				} catch (Exception e) { 
					Debug.Log ("SOOMLA/UNITY " + e.Message); 
				}										// if the purchases can be completed sucessfully
			} 
			catch (Exception e) 
			{																						// if the purchase cannot be completed trigger an error message connectivity issue, IAP doesnt exist on ItunesConnect, etc...)
				Debug.Log ("SOOMLA/UNITY" + e.Message);							
			}
		}
		public void Pack3(){
			try {
				Debug.Log("attempt to purchase");

				StoreInventory.BuyItem ("bretthart_item_id");										// if the purchases can be completed sucessfully
			} 
			catch (Exception e) 
			{																						// if the purchase cannot be completed trigger an error message connectivity issue, IAP doesnt exist on ItunesConnect, etc...)
				Debug.Log ("SOOMLA/UNITY" + e.Message);							
			}
		}public void Pack4(){
			try {
				Debug.Log("attempt to purchase");

				StoreInventory.BuyItem ("casultybundle_item_id");										// if the purchases can be completed sucessfully
			} 
			catch (Exception e) 
			{																						// if the purchase cannot be completed trigger an error message connectivity issue, IAP doesnt exist on ItunesConnect, etc...)
				Debug.Log ("SOOMLA/UNITY" + e.Message);							
			}
		}
		public void Pack5(){
			try {
				Debug.Log("attempt to purchase");

				StoreInventory.BuyItem ("kevinhartbundle_item_id");										// if the purchases can be completed sucessfully
			} 
			catch (Exception e) 
			{																						// if the purchase cannot be completed trigger an error message connectivity issue, IAP doesnt exist on ItunesConnect, etc...)
				Debug.Log ("SOOMLA/UNITY" + e.Message);							
			}
		}
		public void Pack6(){
			try {
				Debug.Log("attempt to purchase");

				StoreInventory.BuyItem ("bigmanbundle_item_id");										// if the purchases can be completed sucessfully
			} 
			catch (Exception e) 
			{																						// if the purchase cannot be completed trigger an error message connectivity issue, IAP doesnt exist on ItunesConnect, etc...)
				Debug.Log ("SOOMLA/UNITY" + e.Message);							
			}
		}
		public void Pack7(){
			try {
				Debug.Log("attempt to purchase");

				StoreInventory.BuyItem ("smallmanbundle_item_id");										// if the purchases can be completed sucessfully
			} 
			catch (Exception e) 
			{																						// if the purchase cannot be completed trigger an error message connectivity issue, IAP doesnt exist on ItunesConnect, etc...)
				Debug.Log ("SOOMLA/UNITY" + e.Message);							
			}
		}
		public void Pack8(){
			try {
				Debug.Log("attempt to purchase");

				StoreInventory.BuyItem ("doublecookiesbooster_item_id");										// if the purchases can be completed sucessfully
			} 
			catch (Exception e) 
			{																						// if the purchase cannot be completed trigger an error message connectivity issue, IAP doesnt exist on ItunesConnect, etc...)
				Debug.Log ("SOOMLA/UNITY" + e.Message);							
			}
		}

	
	} 

} 
