using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public Root SelectedRoot;
    [HideInInspector] public RootSelectionType SelectedType;

    [HideInInspector] public Vector2 AimDir;
    [SerializeField ] private PlayerUIEvents _playerUIEvents;

    [SerializeField] private float storedDelay;

    private bool holding = false;
    private Root storedRoot;
    private float storedTimer;

    private RootMovementController con;

    private bool FireHold = false;
    private bool AltFireHold = false;

    private Vector2 MousePos;
    private Vector2 MouseDelta;

    private PlayerInputActions input;
    private PlayerObject _player;
    private InputAction fireAction;
    private InputAction fireHoldAction;
    private InputAction altFireAction;
    private InputAction altFireHoldAction;
    private InputAction mousePosAction;
    private InputAction mouseDeltaAction;
    private InputAction clearAllAction;
    private InputAction menuAction;

    private Root highlighted = null;

    private void Awake()
    {
        input = new PlayerInputActions();
        con = GetComponent<RootMovementController>();
        _player = GetComponent<PlayerObject>();

        SelectedType = RootSelectionType.NONE;

        CinemachineCore.CameraUpdatedEvent.AddListener(InputUpdate);
    }

    private void OnEnable()
    {
        fireAction = input.Player.Fire; fireAction.Enable();
        fireAction.performed += Fire;

        fireHoldAction = input.Player.FireHold; fireHoldAction.Enable();

        altFireAction = input.Player.AltFire; altFireAction.Enable();
        altFireAction.performed += AltFire;

        altFireHoldAction = input.Player.AltFireHold; altFireHoldAction.Enable();

        mousePosAction = input.Player.MousePosition; mousePosAction.Enable();

        mouseDeltaAction = input.Player.MouseDelta; mouseDeltaAction.Enable();

        clearAllAction = input.Player.ClearAll; clearAllAction.Enable();
        clearAllAction.performed += ClearAll;

        menuAction = input.Player.Menu; menuAction.Enable();
        menuAction.performed += OpenMenu;
    }

    private void OnDisable()
    {
        fireAction.Disable();
        fireAction.performed -= Fire;
        fireHoldAction.Disable();
        altFireAction.Disable();
        altFireHoldAction.Disable();
        altFireHoldAction.performed -= AltFire;
        mousePosAction.Disable();
        mouseDeltaAction.Disable();
        clearAllAction.Disable();
        clearAllAction.performed -= ClearAll;
        menuAction.Disable();
        menuAction.performed -= OpenMenu;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (GameState.IsPaused || _player.UnitStats.IsDead) return;

        // deal with edge cases
        if (SelectedType != RootSelectionType.NONE) 
        {
            storedRoot = null;
            return;
        }

        if (AltFireHold) return;

        // check if over existing branch
        Vector2 mPos = con.cam.ScreenToWorldPoint(MousePos);
        foreach (Root r in con.Roots)
        {
            if (r.EdgeCollider.OverlapPoint(mPos))
            {
                SelectedRoot = r;
                SelectedType = RootSelectionType.MOVE;
                ChangeHighlight(SelectedRoot);
                return;
            }
        }

        // create new branch
        Vector2 playerScreenPos = con.cam.WorldToScreenPoint(transform.position);
        Vector2 dir = (MousePos - playerScreenPos).normalized;

        storedRoot = con.ConstructRoot(dir);
        storedTimer = 0.0f;
        ChangeHighlight(storedRoot);
    }

    private void AltFire(InputAction.CallbackContext context)
    {
        if (GameState.IsPaused || _player.UnitStats.IsDead) return;

        // deal with edge cases
        if (SelectedType != RootSelectionType.NONE) return;
        if (FireHold) return;

        Vector2 mPos = con.cam.ScreenToWorldPoint(MousePos);

        // if clicked on player
        if (con.circleCollider.OverlapPoint(mPos))
        {
            con.ClearAllRoots();
            return;
        }

        // if clicked on grabbing branch
        foreach (Root r in con.GrabRoots)
        {
            if (r.EdgeCollider.OverlapPoint(mPos))
            {
                SelectedRoot = r;
                SelectedType = RootSelectionType.PULL;

                holding = true;

                con.StartPull();
                ChangeHighlight(SelectedRoot);
                return;
            }
        }

        // if clicked on fixed branch
        foreach (Root r in con.Roots)
        {
            if (r.EdgeCollider.OverlapPoint(mPos))
            {
                SelectedRoot = r;
                SelectedType = RootSelectionType.PULL;

                holding = true;

                con.StartPull();
                ChangeHighlight(SelectedRoot);
                return;
            }
        }
    }

    private void ClearAll(InputAction.CallbackContext context)
    {
        con.ClearAllRoots();
    }

    private void OpenMenu(InputAction.CallbackContext context)
    {
        _playerUIEvents.OnMenu();
    }

    private void InputUpdate(CinemachineBrain arg0)
    {
        if (GameState.IsPaused || _player.UnitStats.IsDead) return;

        // get input
        FireHold = (fireHoldAction.ReadValue<float>() >= 0.5f);
        AltFireHold = (altFireHoldAction.ReadValue<float>() >= 0.5f);

        MousePos = mousePosAction.ReadValue<Vector2>();
        MouseDelta = mouseDeltaAction.ReadValue<Vector2>();

        Vector2 playerScreenPos = con.cam.WorldToScreenPoint(transform.position);
        AimDir = (MousePos - playerScreenPos).normalized;

        if (SelectedType == RootSelectionType.MOVE) 
        { 
            if (FireHold)
            {
                Vector2 MouseWorldPos = con.cam.ScreenToWorldPoint(MousePos);
                foreach (Root r in con.Roots)
                {
                    if (r.EdgeCollider.OverlapPoint(MouseWorldPos))
                    {
                        SelectedRoot = r;
                        ChangeHighlight(r);
                        break;
                    }
                }
            }
            else
            {
                SelectedRoot = null;
                SelectedType = RootSelectionType.NONE;
                ChangeHighlight(null);
            }
        }
        else if (SelectedType == RootSelectionType.NONE && FireHold) 
        {
            storedTimer += Time.deltaTime;

            Vector2 MouseWorldPos = con.cam.ScreenToWorldPoint(MousePos);

            if (storedTimer > storedDelay)
            {
                foreach (Root r in con.GrabRoots)
                {
                    if (r == storedRoot)
                    {
                        SelectedRoot = r;
                        SelectedType = RootSelectionType.PULL;

                        holding = true;

                        con.StartPull();
                        ChangeHighlight(SelectedRoot);
                        break;
                    }
                }
            }

            // if still nothing selected
            if (SelectedType == RootSelectionType.NONE)
            {
                foreach (Root r in con.Roots)
                {
                    if (r.EdgeCollider.OverlapPoint(MouseWorldPos))
                    {
                        SelectedRoot = r;
                        SelectedType = RootSelectionType.MOVE;
                        ChangeHighlight(SelectedRoot);
                        break;
                    }
                }
            }
        }


        if (AltFireHold)
        {
            if (!holding)
            {
                Vector2 MouseWorldPos = con.cam.ScreenToWorldPoint(MousePos);
                Vector2 LastMouseWorldPos = con.cam.ScreenToWorldPoint(MousePos - MouseDelta);

                // right click sweep
                RaycastHit2D[] hits = Physics2D.LinecastAll(LastMouseWorldPos, MouseWorldPos, con.rootMask);
                foreach (RaycastHit2D h in hits)
                {
                    if (!h.collider.transform.CompareTag("Root")) continue;

                    Root r = h.collider.transform.GetComponent<Root>();
                    if (r == SelectedRoot || (!con.Roots.Contains(r) && !con.GrabRoots.Contains(r))) continue;
                    con.ClearRoot(r);
                }
            }
        }
        else if (SelectedType == RootSelectionType.PULL && (SelectedRoot.Fixed || !FireHold))
        {
            con.EndPull();
            SelectedRoot = null;
            SelectedType = RootSelectionType.NONE;
            holding = false;
            ChangeHighlight(null);
        }

        if (!FireHold && !AltFireHold)
        {
            Root highR = null;

            Vector2 MouseWorldPos = con.cam.ScreenToWorldPoint(MousePos);
            foreach (Root r in con.Roots)
            {
                if (r.EdgeCollider.OverlapPoint(MouseWorldPos))
                {
                    highR = r;
                    break;
                }
            }

            if (highR == null)
            {
                foreach (Root r in con.GrabRoots)
                {
                    if (r.EdgeCollider.OverlapPoint(MouseWorldPos))
                    {
                        highR = r;
                        break;
                    }
                }
            }

            ChangeHighlight(highR);
        }
    }


    private void ChangeHighlight(Root root)
    {
        if (highlighted == root) return;

        if (highlighted != null) highlighted.Unhighlight();

        if (root!= null) root.Highlight();

        highlighted = root;
    }

}

public enum RootSelectionType
{
    PULL,
    MOVE,
    NONE
}
