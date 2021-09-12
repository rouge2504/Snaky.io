using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.UI;
#if UIP
/// <summary>
/// An example of basic Unity IAP functionality.
/// To use with your account, configure the product ids (AddProduct)
/// and Google Play key (SetPublicKey).
/// </summary>
public class UnityIAP : MonoBehaviour, IStoreListener     //Commented out recently
{
    // Indicates if IAP has been initialized. Establishes a singleton
    // design pattern.
    // Disregards initialization success / failure because failure
    // generally indicates a programming or administrative error
    // not fixable by the user.  
    public static UnityIAP instance = null;
    // Unity IAP objects 
    private IStoreController m_Controller;
    private IAppleExtensions m_AppleExtensions;   //recent

    private void Awake()
    {
        // Initialize IAP once.
        if (UnityIAP.instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeIAP();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeIAP()
    {
        ConfigurationBuilder builder;
        /*if (Application.platform == RuntimePlatform.Android)
        {
            builder = ConfigurationBuilder.Instance(
                Google.Play.Billing.GooglePlayStoreModule.Instance());
        }
        else
        {*/
            builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        //}
        //        builder.AddProduct("coins", ProductType.Consumable, new IDs
        //        {
        //            {"com.unity3d.unityiap.unityiapdemo.100goldcoins.v2.c", GooglePlay.Name},
        //            {"com.unity3d.unityiap.unityiapdemo.100goldcoins.6", AppleAppStore.Name},
        //            {"com.unity3d.unityiap.unityiapdemo.100goldcoins.mac", MacAppStore.Name},
        //            {"com.unity3d.unityiap.unityiapdemo.100goldcoins.win8", WinRT.Name}
        //        });
#if UNITY_ANDROID
        builder.AddProduct("com.camc.slitherio.snakes.worms.adremove", ProductType.NonConsumable, new IDs     //recent
                {
                    {"com.camc.slitherio.snakes.worms.adremove", GooglePlay.Name},
                    {"com.camc.slitherio.snakes.worms.adremover" , AppleAppStore.Name }
                    //{"com.camc.slither.adremover", AppleAppStore.Name}
                });
        builder.AddProduct("com.camc.slitherio.snakes.worms.usetransparency", ProductType.NonConsumable, new IDs
                {
                    {"com.camc.slitherio.snakes.worms.usetransparency", GooglePlay.Name},
                    {"com.camc.slitherio.snakes.worms.tranparency", AppleAppStore.Name }
                    //{ "com.camcgames.slither.transparency", AppleAppStore.Name}
                });

        builder.AddProduct("com.camc.slitherio.snakes.worms.babysnake", ProductType.NonConsumable, new IDs
                {
                    {"com.camc.slitherio.snakes.worms.babysnake", GooglePlay.Name},
                    {"com.camc.slitherio.snakes.worms.babysnake", AppleAppStore.Name }
                    //{ "com.camcgames.slither.transparency", AppleAppStore.Name}
                });        //end recent
        builder.AddProduct("com.camc.slitherio.snakes.worms.bundle", ProductType.NonConsumable, new IDs
                {
                    {"com.camc.slitherio.snakes.worms.bundle", GooglePlay.Name},
                    {"com.camc.slitherio.snakes.worms.bundle", AppleAppStore.Name }
                    //{ "com.camcgames.slither.transparency", AppleAppStore.Name}
                });        //end recent
        builder.AddProduct("com.camc.slitherio.snakes.worms.egg1", ProductType.Consumable, new IDs
                {
                    {"com.camc.slitherio.snakes.worms.egg1", GooglePlay.Name},
                    {"com.camc.slitherio.snakes.worms.egg1", AppleAppStore.Name }
                    //{ "com.camcgames.slither.transparency", AppleAppStore.Name}
                });        //end recent
        builder.AddProduct("com.camc.slitherio.snakes.worms.egg2", ProductType.Consumable, new IDs
                {
                    {"com.camc.slitherio.snakes.worms.egg2", GooglePlay.Name},
                    {"com.camc.slitherio.snakes.worms.egg2", AppleAppStore.Name }
                    //{ "com.camcgames.slither.transparency", AppleAppStore.Name}
                });        //end recent
        builder.AddProduct("com.camc.slitherio.snakes.worms.egg3", ProductType.Consumable, new IDs
                {
                    {"com.camc.slitherio.snakes.worms.egg3", GooglePlay.Name},
                    {"com.camc.slitherio.snakes.worms.egg3", AppleAppStore.Name }
                    //{ "com.camcgames.slither.transparency", AppleAppStore.Name}
                });        //end recent
        builder.AddProduct("android.test.purchased", ProductType.Consumable, new IDs     //recent
                {
                    {"android.test.purchased", GooglePlay.Name},
                    {"com.camc.slitherio.snakes.worms.adremover" , AppleAppStore.Name }
                    //{"com.camc.slither.adremover", AppleAppStore.Name}
                });
#endif

#if UNITY_IPHONE
        builder.AddProduct("com.camc.removeads", ProductType.NonConsumable, new IDs     //recent
                {
                    {"com.camc.removeads", GooglePlay.Name},
                    {"com.camc.removeads" , AppleAppStore.Name }
                    //{"com.camc.slither.adremover", AppleAppStore.Name}
                });
        builder.AddProduct("com.camc.usetransparency", ProductType.NonConsumable, new IDs
                {
                    {"com.camc.usetransparency", GooglePlay.Name},
                    {"com.camc.usetransparency", AppleAppStore.Name }
             });
        
          builder.AddProduct("com.camc.babysnake", ProductType.NonConsumable, new IDs
                {
                    {"com.camc.babysnake", GooglePlay.Name},
                    {"com.camc.babysnake", AppleAppStore.Name }
             });
        builder.AddProduct("com.camc.bundle", ProductType.NonConsumable, new IDs
                {
                    {"com.camc.bundle", GooglePlay.Name},
                    {"com.camc.bundle", AppleAppStore.Name }
             });
          builder.AddProduct("com.camc.egg1", ProductType.Consumable, new IDs
                {
                    {"com.camc.egg1", GooglePlay.Name},
                    {"com.camc.egg1", AppleAppStore.Name }
             });
         builder.AddProduct("com.camc.egg2", ProductType.Consumable, new IDs
                {
                    {"com.camc.egg2", GooglePlay.Name},
                    {"com.camc.egg2", AppleAppStore.Name }
             });
         builder.AddProduct("com.camc.egg3", ProductType.Consumable, new IDs
                {
                    {"com.camc.egg3", GooglePlay.Name},
                    {"com.camc.egg3", AppleAppStore.Name }
             });
        //{ "com.camcgames.slither.transparency", AppleAppStore.Name}
#endif
        //        builder.AddProduct("subscription", ProductType.Subscription, new IDs
        //        {
        //            {"com.unity3d.unityiap.unityiapdemo.subscription", GooglePlay.Name, AppleAppStore.Name}
        //        });

        // Now we're ready to initialize Unity IAP.
        UnityPurchasing.Initialize(this, builder);    //recent
    }

    /// <summary>
    /// This will be called when Unity IAP has finished initialising.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_Controller = controller;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();  //recent
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                // Ask the user if billing is disabled in device settings.
                break;
            case InitializationFailureReason.NoProductsAvailable:
                // Developer configuration error; check product metadata.
                break;
        }
    }

    /// <summary>
    /// This will be called when a purchase completes.
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        #if EVENTS
        Dictionary<string, object> vals = new Dictionary<string, object>();
        vals.Add("inapp_id", e.purchasedProduct.definition.id);
        vals.Add("currency", e.purchasedProduct.metadata.isoCurrencyCode);
        vals.Add("price", (float)e.purchasedProduct.metadata.localizedPrice);
        #endif
      //  Debug.Log("purchased "+ e.purchasedProduct.definition.id);
        if (e.purchasedProduct.definition.id == "android.test.purchased" || e.purchasedProduct.definition.id == "com.camc.removeads") //adremove
        {
#if EVENTS
            vals.Add("inapp_type", "test id");
#endif
            StoreManager.instance.OnSuccessfulPurchaseEggPack(2);
        }
        else if (e.purchasedProduct.definition.id == "com.camc.slitherio.snakes.worms.adremove" || e.purchasedProduct.definition.id == "com.camc.removeads") //adremove
        {
#if EVENTS
            vals.Add("inapp_type", "ad remove");
#endif
            StoreManager.instance.OnSuccessfulPurchaseRemoveAD();
        }

        if (e.purchasedProduct.definition.id == "com.camc.slitherio.snakes.worms.usetransparency"|| e.purchasedProduct.definition.id == "com.camc.usetransparency")  //usetransparency
        {
#if EVENTS
            vals.Add("inapp_type", "transprency");
#endif
            StoreManager.instance.OnSuccessfulPurchaseTransperancy();
        }

        if (e.purchasedProduct.definition.id == "com.camc.slitherio.snakes.worms.babysnake" || e.purchasedProduct.definition.id == "com.camc.babysnake")  //usetransparency
        {
#if EVENTS
            vals.Add("inapp_type", "baby snake");
#endif
            StoreManager.instance.OnSuccessfulPurchaseBabySnake();
        }

        if (e.purchasedProduct.definition.id == "com.camc.slitherio.snakes.worms.bundle" || e.purchasedProduct.definition.id == "com.camc.bundle")  //usetransparency
        {
#if EVENTS
            vals.Add("inapp_type", "bundle");
#endif
            StoreManager.instance.OnSuccessfulPurchaseBundle();
        }

        if (e.purchasedProduct.definition.id == "com.camc.slitherio.snakes.worms.egg1" || e.purchasedProduct.definition.id == "com.camc.egg1")  //usetransparency
        {
#if EVENTS
            vals.Add("inapp_type", "egg1");
#endif
            StoreManager.instance.OnSuccessfulPurchaseEggPack(11);
        }

        if (e.purchasedProduct.definition.id == "com.camc.slitherio.snakes.worms.egg2" || e.purchasedProduct.definition.id == "com.camc.egg2")  //usetransparency
        {
#if EVENTS
            vals.Add("inapp_type", "egg2");
#endif
            StoreManager.instance.OnSuccessfulPurchaseEggPack(25);
        }

        if (e.purchasedProduct.definition.id == "com.camc.slitherio.snakes.worms.egg3" || e.purchasedProduct.definition.id == "com.camc.egg3")  //usetransparency
        {
#if EVENTS
            vals.Add("inapp_type", "egg3");
#endif
            StoreManager.instance.OnSuccessfulPurchaseEggPack(40);
        }
#if EVENTS

        if (PlayerPrefs.GetInt("firstTime", 1) == 0)
        {
            AppMetrica.Instance.ReportEvent("payment_succeed", vals);
            PlayerPrefs.SetInt("noAdsButton", 1);
        }
        else
            AppMetrica.Instance.ReportEvent("payment_restored", vals);

        var product = e.purchasedProduct;
     //   if (String.Equals(product.definition.id, e.purchasedProduct.definition.id, StringComparison.Ordinal))
      //  {
            string currency = product.metadata.isoCurrencyCode;
            decimal price = product.metadata.localizedPrice;

            // Creating the instance of the YandexAppMetricaRevenue class.
            YandexAppMetricaRevenue revenue = new YandexAppMetricaRevenue(price, currency);
            if (product.receipt != null)
            {
                // Creating the instance of the YandexAppMetricaReceipt class.
                YandexAppMetricaReceipt yaReceipt = new YandexAppMetricaReceipt();
                Receipt receipt = JsonUtility.FromJson<Receipt>(product.receipt);
#if UNITY_ANDROID
                PayloadAndroid payloadAndroid = JsonUtility.FromJson<PayloadAndroid>(receipt.Payload);
                yaReceipt.Signature = payloadAndroid.Signature;
                yaReceipt.Data = payloadAndroid.Json;
#elif UNITY_IPHONE
            yaReceipt.TransactionID = receipt.TransactionID;
            yaReceipt.Data = receipt.Payload;
#endif
                revenue.Receipt = yaReceipt;
            }
            // Sending data to the AppMetrica server.
            AppMetrica.Instance.ReportRevenue(revenue);
     //   }
#endif
        // Indicate we have handled this purchase, we will not be informed of it again.
        return PurchaseProcessingResult.Complete;
    }
#if EVENTS
    [System.Serializable]
    public struct Receipt
    {
        public string Store;
        public string TransactionID;
        public string Payload;
    }

    // Additional information about the IAP for Android.
    [System.Serializable]
    public struct PayloadAndroid
    {
        public string Json;
        public string Signature;
    }

  
#endif
    /// <summary>
    /// This will be called when an attempted purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
    {
        Debug.Log("Purchase Failed");
    }

    /// <summary>
    /// This will be called after a call to <extension>.RestoreTransactions().
    /// </summary>
    private void OnTransactionsRestored(bool success)
    {
        if (success)
        {
            Debug.Log("Transactions restored.");
            //  GameManager.instance.OnSuccessfulPurchase();
        }
        else
        {
            Debug.Log("Transactions restored failed.");
        }
    }

    /// <summary>
    /// iOS Specific.
    /// This is called as part of Apple's 'Ask to buy' functionality,
    /// when a purchase is requested by a minor and referred to a parent
    /// for approval.
    /// 
    /// When the purchase is approved or rejected, the normal purchase events
    /// will fire.
    /// </summary>
    /// <param name="item">Item.</param>
    private void OnDeferred(Product item)
    {
        //        Debug.Log("Purchase deferred: " + item.definition.id);
    }

    public void BuyItem(string productID)
    {
        PlayerPrefs.SetInt("firstTime", 0);
        Dictionary<string, string> eventDict = new Dictionary<string, string>();
        eventDict.Add(productID, "BuyItem");
        //AppsFlyerSDK.AppsFlyer.sendEvent(productID, eventDict);
        Debug.Log("Buy Item: " + productID);
        m_Controller.InitiatePurchase(productID);
        //RojoDev
    }

    public void RestorePurchase()
    {
        m_AppleExtensions.RestoreTransactions(OnTransactionsRestored);    //recent
    }
}
#endif
