﻿using BepInEx;
using System;
using Unbound.Cards.Utils;
using Unbound.Core;
using Unbound.Core.Networking;
using Unbound.Core.Utils;
using Unbound.Core.Utils.UI;
using UnityEngine;

namespace Unbound.Cards
{
    [BepInDependency("dev.rounds.unbound.core")]
    [BepInPlugin("dev.rounds.unbound.cards", "Unbound Lib Cards", "1.0.0")]
    public class UnboundCards : BaseUnityPlugin {
        public static UnboundCards instance;
        public static CardInfo templateCard;
        void Awake()
        {
            instance = this;
            var hamony = new HarmonyLib.Harmony("dev.rounds.unbound.cards");
            hamony.PatchAll();

            gameObject.AddComponent<CardManager>();
            gameObject.AddComponent<ToggleCardsMenuHandler>();

            // fetch card to use as a template for all custom cards
            templateCard = Resources.Load<GameObject>("0 Cards/0. PlainCard").GetComponent<CardInfo>();
            templateCard.allowMultiple = true;
        }

        void Start()
        {
            CardManager.defaultCards = CardChoice.instance.cards;

            // register default cards with toggle menu
            foreach (var card in CardManager.defaultCards)
            {
                CardManager.cards.Add(card.name,
                    new Card("Vanilla", UnboundCore.config.Bind("Cards: Vanilla", card.name, true), card));

                var networkEvents = gameObject.GetOrAddComponent<NetworkEventCallbacks>();
                networkEvents.OnJoinedRoomEvent += CardManager.OnJoinedRoomAction;
                networkEvents.OnLeftRoomEvent += CardManager.OnLeftRoomAction;
            }
        }

        bool reg = false;
        void Update() {
            GameManager.lockInput = GameManager.lockInput || ToggleCardsMenuHandler.menuOpenFromOutside;

            if (reg) {
                Debug.Log("Already registered");
                return;
            } else if (!ToggleCardsMenuHandler.cardMenuCanvas) {
                Debug.Log("No card menu canvas");
                return;
            }
            ModOptions.RegesterPrioritySubMenu("Toggle Cards",
                () => {
                    Debug.Log("Toggle Cards");
                    ToggleCardsMenuHandler.SetActive(ToggleCardsMenuHandler.cardMenuCanvas.transform, true);
                });
        }

        public static void AddAllCardsCallback(Action<CardInfo[]> callback)
        {
            CardManager.AddAllCardsCallback(callback);
        }
    }
}
