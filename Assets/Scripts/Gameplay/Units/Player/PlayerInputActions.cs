//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Scripts/Gameplay/Units/Player/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""63039e2a-2edb-42cb-8b68-ff95af357044"",
            ""actions"": [
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""7edb84c0-ba61-40fe-8cdd-ed5249ebaaed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""FireHold"",
                    ""type"": ""Value"",
                    ""id"": ""d25e5999-b9fb-4d2f-a1a4-e72699644e63"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Hold"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""AltFire"",
                    ""type"": ""Button"",
                    ""id"": ""71e44dce-5aae-4985-8e3a-a68153b6e9aa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AltFireHold"",
                    ""type"": ""Value"",
                    ""id"": ""06552072-a2f6-4933-be4f-d4b698ad028a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Hold"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""fbef1310-3fae-44f7-8275-32e07935b621"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MouseDelta"",
                    ""type"": ""Value"",
                    ""id"": ""f1327092-a6db-4467-9cb8-6a12a4d69d69"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ClearAll"",
                    ""type"": ""Button"",
                    ""id"": ""a9860c59-3d49-4cc5-917a-a5b352de479e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""538f3dea-dc77-4bac-b5f7-9eaf612b7941"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""dc4e5bcd-914a-4ecc-97c8-3bc0a37e6196"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4dd1a5d4-f3fa-4b01-ad5c-75ab618a26bb"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AltFire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ade7c415-00da-4368-b037-7cfb1e0b2ce6"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb9a11ca-8d29-4220-b6dd-661a733c968e"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7975623-2011-4631-81b9-306c71171f13"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AltFireHold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ebff8b9-8341-43f8-a545-14ce3ab91344"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FireHold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3abd6c4-8419-4f91-9c19-7d4f624e5ff3"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ClearAll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cec418e3-6b69-4ae5-b27e-c1120403437e"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Fire = m_Player.FindAction("Fire", throwIfNotFound: true);
        m_Player_FireHold = m_Player.FindAction("FireHold", throwIfNotFound: true);
        m_Player_AltFire = m_Player.FindAction("AltFire", throwIfNotFound: true);
        m_Player_AltFireHold = m_Player.FindAction("AltFireHold", throwIfNotFound: true);
        m_Player_MousePosition = m_Player.FindAction("MousePosition", throwIfNotFound: true);
        m_Player_MouseDelta = m_Player.FindAction("MouseDelta", throwIfNotFound: true);
        m_Player_ClearAll = m_Player.FindAction("ClearAll", throwIfNotFound: true);
        m_Player_Menu = m_Player.FindAction("Menu", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Fire;
    private readonly InputAction m_Player_FireHold;
    private readonly InputAction m_Player_AltFire;
    private readonly InputAction m_Player_AltFireHold;
    private readonly InputAction m_Player_MousePosition;
    private readonly InputAction m_Player_MouseDelta;
    private readonly InputAction m_Player_ClearAll;
    private readonly InputAction m_Player_Menu;
    public struct PlayerActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Fire => m_Wrapper.m_Player_Fire;
        public InputAction @FireHold => m_Wrapper.m_Player_FireHold;
        public InputAction @AltFire => m_Wrapper.m_Player_AltFire;
        public InputAction @AltFireHold => m_Wrapper.m_Player_AltFireHold;
        public InputAction @MousePosition => m_Wrapper.m_Player_MousePosition;
        public InputAction @MouseDelta => m_Wrapper.m_Player_MouseDelta;
        public InputAction @ClearAll => m_Wrapper.m_Player_ClearAll;
        public InputAction @Menu => m_Wrapper.m_Player_Menu;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Fire.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                @FireHold.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFireHold;
                @FireHold.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFireHold;
                @FireHold.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFireHold;
                @AltFire.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAltFire;
                @AltFire.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAltFire;
                @AltFire.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAltFire;
                @AltFireHold.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAltFireHold;
                @AltFireHold.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAltFireHold;
                @AltFireHold.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAltFireHold;
                @MousePosition.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @MouseDelta.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseDelta;
                @MouseDelta.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseDelta;
                @MouseDelta.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseDelta;
                @ClearAll.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClearAll;
                @ClearAll.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClearAll;
                @ClearAll.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClearAll;
                @Menu.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMenu;
                @Menu.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMenu;
                @Menu.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMenu;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @FireHold.started += instance.OnFireHold;
                @FireHold.performed += instance.OnFireHold;
                @FireHold.canceled += instance.OnFireHold;
                @AltFire.started += instance.OnAltFire;
                @AltFire.performed += instance.OnAltFire;
                @AltFire.canceled += instance.OnAltFire;
                @AltFireHold.started += instance.OnAltFireHold;
                @AltFireHold.performed += instance.OnAltFireHold;
                @AltFireHold.canceled += instance.OnAltFireHold;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @MouseDelta.started += instance.OnMouseDelta;
                @MouseDelta.performed += instance.OnMouseDelta;
                @MouseDelta.canceled += instance.OnMouseDelta;
                @ClearAll.started += instance.OnClearAll;
                @ClearAll.performed += instance.OnClearAll;
                @ClearAll.canceled += instance.OnClearAll;
                @Menu.started += instance.OnMenu;
                @Menu.performed += instance.OnMenu;
                @Menu.canceled += instance.OnMenu;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnFire(InputAction.CallbackContext context);
        void OnFireHold(InputAction.CallbackContext context);
        void OnAltFire(InputAction.CallbackContext context);
        void OnAltFireHold(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnMouseDelta(InputAction.CallbackContext context);
        void OnClearAll(InputAction.CallbackContext context);
        void OnMenu(InputAction.CallbackContext context);
    }
}