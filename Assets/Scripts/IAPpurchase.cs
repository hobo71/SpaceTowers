using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using LitJson;
using System;
using Soomla;

namespace Soomla.Store.Example {
	public class IAPpurchase : MonoBehaviour {
		
		private static IAPpurchase instance = null;

		public string credit2 = "2 credit_item_id";
		public string credit4 = "4 credit_item_id";
		public string credit10 = "10 credit_item_id";
		public string credit20 = "20 credit_item_id";
		public string credit40 = "40+2 credit_item_id";
		public string credit100 = "100+6 credit_item_id";
		public string credit200 = "200+10 credit_item_id";
		public string credit300 = "300+16 credit_item_id";
		public string credit500 = "500+40 credit_item_id";

		void Awake(){
			if(instance == null){ 	//making sure we only initialize one instance.
				instance = this;
				GameObject.DontDestroyOnLoad(this.gameObject);
			} else {					//Destroying unused instances.
				GameObject.Destroy(this);
			}
		}

		// Use this for initialization
		void Start () {
			StoreEvents.OnSoomlaStoreInitialized += onSoomlaStoreInitialized;
			StoreEvents.OnUnexpectedStoreError += onUnexpectedStoreError;
			StoreEvents.OnItemPurchased += onItemPurchased;
			SoomlaStore.Initialize(new MuffinRushAssets());
		}

		// Update is called once per frame
		void Update () {

		}

		public void exitdialog(GameObject Dialog,GameObject transImg)
		{
			Dialog.SetActive (false);
			transImg.SetActive (false);
		}

		public void IAPButtonClick(string IAPProductID)
		{
			try {
				Debug.Log("attempt to purchase");

				StoreInventory.BuyItem (IAPProductID);										// if the purchases can be completed sucessfully
			} 
			catch (Exception e) 
			{																						// if the purchase cannot be completed trigger an error message connectivity issue, IAP doesnt exist on ItunesConnect, etc...)
				Debug.Log ("SOOMLA/UNITY" + e.Message);							
			}
		}

		public void onUnexpectedStoreError(int errorCode) {
			SoomlaUtils.LogError ("ExampleEventHandler", "error with code: " + errorCode);
		}

		public void onSoomlaStoreInitialized() {

			if (StoreInfo.Currencies.Count>0) {
				try {

				} catch (VirtualItemNotFoundException ex){
					SoomlaUtils.LogError("SOOMLA ExampleWindow", ex.Message);
				}
			}
		}

		public void onItemPurchased(PurchasableVirtualItem pvi, string payload) {
			print ("PurchasePayload: " + payload);
			print ("ItemPurchased: " + pvi.ItemId + " Name: " + pvi.Name);
			if (pvi.ItemId.Equals(credit2)) 
			{
				StartCoroutine (Update_Credit_Event(2));
				print("Purchase Sucessfully item : "+credit2);
			}
			if (pvi.ItemId.Equals(credit4)) 
			{
				StartCoroutine (Update_Credit_Event(4));
				print("Purchase Sucessfully item : "+credit4);
			}
			if (pvi.ItemId.Equals(credit10))
			{
				StartCoroutine (Update_Credit_Event(10));
				print("Purchase Sucessfully item : "+credit10);
			}
			if (pvi.ItemId.Equals(credit20)) 
			{
				StartCoroutine (Update_Credit_Event(20));
				print("Purchase Sucessfully item : "+credit20);
			}
			if (pvi.ItemId.Equals(credit40)) 
			{
				StartCoroutine (Update_Credit_Event(42));
				print("Purchase Sucessfully item : "+credit40);
			}
			if (pvi.ItemId.Equals(credit100)) 
			{
				StartCoroutine (Update_Credit_Event(106));
				print("Purchase Sucessfully item : "+credit100);
			}
			if (pvi.ItemId.Equals(credit200)) 
			{
				StartCoroutine (Update_Credit_Event(210));
				print("Purchase Sucessfully item : "+credit200);
			}
			if (pvi.ItemId.Equals(credit300)) 
			{
				StartCoroutine (Update_Credit_Event(316));
				print("Purchase Sucessfully item : "+credit300);
			}
			if (pvi.ItemId.Equals (credit500))
			{
				StartCoroutine (Update_Credit_Event(540));
				print("Purchase Sucessfully item : "+credit500);
			}

			//	if (CamManager.isInternetConnected() && Constants.IsFbUser) {
			//			UpdateStorage.UpdateData();
			//		}
		}

		IEnumerator Update_Credit_Event(int NoOfCredits)
		{
			LoadingBar.isVisible = true;
			string ourPostData = UpdateScoreToJson(NoOfCredits);
			Dictionary<string,string> headers = new Dictionary<string,string>();
			headers.Add("Content-Type", "application/json");
			byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
			WWW www = new WWW(Constants.hostName+"update_credit", pData, headers);
			print ("Loading.....");
			yield return www;
			print ("Data Loaded ."+www.text);
			JsonData jsonResponse = JsonMapper.ToObject(www.text);
			string isSuccessRes = (string)jsonResponse["isSuccess"];
			LoadingBar.isVisible = false;
			if (isSuccessRes == "true") {
				string responseRes = (string)jsonResponse["response"];
				PlayerPrefs.SetInt (Constants.Player_credits, PlayerPrefs.GetInt (Constants.Player_credits) + NoOfCredits);
				print ("Sucess Responce : "+responseRes);
			} else {
				string responseRes = (string)jsonResponse["response"];
				print ("Responce : "+responseRes);
			}
		}

		public  string UpdateScoreToJson(int NoOfCredits) {
			UserDetailValues user = new UserDetailValues();
			user.email  = PlayerPrefs.GetString(Constants.Player_email);
			user.reg_no  = PlayerPrefs.GetInt (Constants.Player_reg_id).ToString();
			user.best_score  = PlayerPrefs.GetInt (Constants.PlayerHighScore).ToString();
			user.credits  = NoOfCredits.ToString();
			string json_user = JsonMapper.ToJson(user); 
			Debug.Log(json_user);
			return json_user;
		}
	}
}
