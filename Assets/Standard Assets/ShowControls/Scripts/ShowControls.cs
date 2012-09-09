using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MouseDirection { None, Horizontal, Vertical, Both }
public enum MouseButton { None, LeftClick, RightClick, BothClick, MiddleClick, ScrollWheel };

public enum ShowControlPosition { Top, Bottom };
public enum ShowControlStyle { Dock, FullScreen  };

public class CustomDisplay
{
    public static CustomDisplay esdf = new CustomDisplay((Texture)Resources.Load("Textures/showControlsSpecialESDF", typeof(Texture)));
    public static CustomDisplay wasd = new CustomDisplay((Texture)Resources.Load("Textures/showControlsSpecialWASD", typeof(Texture)));
    public static CustomDisplay arrows = new CustomDisplay((Texture)Resources.Load("Textures/showControlsSpecialArrows", typeof(Texture)));
    public Texture customTexture;

    public CustomDisplay(Texture tex)
    {
        if (tex == null)
        {
            Debug.Log("CustomDisplay has null texture, trying to load manually");
            tex = (Texture)Resources.Load("Textures/showControlsSpecialWASD.png", typeof(Texture));
            Debug.Log("after reload, tex=" + tex);
        }
        customTexture = tex;
    }
}

public class ControlItem
{
    /* list of keys that should use the "big" key instead of the small.
     * Also has optional ToString() override to make some fit. */
    public static Dictionary<KeyCode, string> BigKeys = new Dictionary<KeyCode, string>()
    {
        { KeyCode.Backspace, "Bksp" },
        { KeyCode.Delete, "Del" },
        { KeyCode.Tab, null },
        { KeyCode.Clear, null },
        { KeyCode.Return, null },
        { KeyCode.Pause, null },
        { KeyCode.Escape, "Esc" },
        { KeyCode.Space, null },
        { KeyCode.Keypad0, "N0" }, /* doesn't NEED to be wide, but it is IRL */
        { KeyCode.KeypadEnter, "NEnter" },
        { KeyCode.Home, null },
        { KeyCode.End, null },
        { KeyCode.PageUp, "PgUp" },
        { KeyCode.PageDown, "PgDn" },
        { KeyCode.Numlock, "NumLk" },
        { KeyCode.CapsLock, "CapsLk" },
        { KeyCode.LeftShift, "L Shift"},
        { KeyCode.LeftControl, "L Ctrl" },
        { KeyCode.RightControl, "R Ctrl" },
        { KeyCode.LeftAlt, "L Alt" },
        { KeyCode.RightAlt, "R Alt" },
        { KeyCode.LeftApple, "L Apple" },
        { KeyCode.RightApple, "R Apple" },
        { KeyCode.LeftWindows, "L Win" },
        { KeyCode.RightWindows, "R Win" },
        { KeyCode.AltGr, "Alt Gr" },
        { KeyCode.Help, null },
        { KeyCode.Print, null },
        { KeyCode.SysReq, null },
        { KeyCode.Break, null },
        { KeyCode.Menu, null }
    };

    /* defines custom strings for some of the small keys */
    public static Dictionary<KeyCode, string> SmallKeys = new Dictionary<KeyCode, string>()
    {
        { KeyCode.Keypad1, "N1" },
        { KeyCode.Keypad2, "N2" },
        { KeyCode.Keypad3, "N3" },
        { KeyCode.Keypad4, "N4" },
        { KeyCode.Keypad5, "N5" },
        { KeyCode.Keypad6, "N6" },
        { KeyCode.Keypad7, "N7" },
        { KeyCode.Keypad8, "N8" },
        { KeyCode.Keypad9, "N9" },
        { KeyCode.KeypadPeriod, "N." },
        { KeyCode.KeypadDivide, "N/" },
        { KeyCode.KeypadMultiply, "N*" },
        { KeyCode.KeypadMinus, "N-" },
        { KeyCode.KeypadPlus, "N+" },
        { KeyCode.KeypadEquals, "N=" },
        { KeyCode.UpArrow, "\u2191" },
        { KeyCode.DownArrow, "\u2193" },
        { KeyCode.LeftArrow, "\u2190" },
        { KeyCode.RightArrow, "\u2192" },
        { KeyCode.Insert, "Ins" },
        { KeyCode.Alpha0, "0" },
        { KeyCode.Alpha1, "1" },
        { KeyCode.Alpha2, "2" },
        { KeyCode.Alpha3, "3" },
        { KeyCode.Alpha4, "4" },
        { KeyCode.Alpha5, "5" },
        { KeyCode.Alpha6, "6" },
        { KeyCode.Alpha7, "7" },
        { KeyCode.Alpha8, "8" },
        { KeyCode.Alpha9, "9" },
        { KeyCode.Exclaim, "!" },
        { KeyCode.DoubleQuote, "\"" },
        { KeyCode.Hash, "#" },
        { KeyCode.Dollar, "$" },
        { KeyCode.Ampersand, "&" },
        { KeyCode.Quote, "'" },
        { KeyCode.LeftParen, "(" },
        { KeyCode.RightParen, ")" },
        { KeyCode.Asterisk, "*" },
        { KeyCode.Plus, "+" },
        { KeyCode.Comma, "," },
        { KeyCode.Minus, "-" },
        { KeyCode.Period, "." },
        { KeyCode.Slash, "/" },
        { KeyCode.Colon, ":" },
        { KeyCode.Semicolon, ";" },
        { KeyCode.Less, "<" },
        { KeyCode.Greater, ">" },
        { KeyCode.Question, "?" },
        { KeyCode.At, "@" },
        { KeyCode.LeftBracket, "[" },
        { KeyCode.Backslash, "\\" },
        { KeyCode.RightBracket, "]" },
        { KeyCode.Caret, "^" },
        { KeyCode.Underscore, "_" },
        { KeyCode.BackQuote, "`" },
    };

    public string description;
    public KeyCode[] keys = null;
    public MouseButton button = MouseButton.None;
    public MouseDirection direction = MouseDirection.None;
    public CustomDisplay custom = null;

    public ControlItem(string description, KeyCode key)
    {
        this.description = description;
        this.keys = new KeyCode[1] { key };
    }
    public ControlItem(string description, KeyCode[] keys)
    {
        this.description = description;
        this.keys = keys;
    }
    public ControlItem(string description, MouseDirection direction)
    {
        this.description = description;
        this.direction = direction;
    }
    public ControlItem(string description, MouseButton button)
    {
        this.description = description;
        this.button = button;
    }
    public ControlItem(string description, MouseDirection direction, MouseButton button)
    {
        this.description = description;
        this.direction = direction;
        this.button = button;
    }
    public ControlItem(string description, KeyCode key, MouseDirection direction)
    {
        this.description = description;
        this.direction = direction;
        this.keys = new KeyCode[1] { key };
    }
    public ControlItem(string description, KeyCode key, MouseButton button)
    {
        this.description = description;
        this.button = button;
        this.keys = new KeyCode[1] { key };
    }
    public ControlItem(string description, KeyCode key, MouseDirection direction, MouseButton button)
    {
        this.description = description;
        this.direction = direction;
        this.button = button;
        this.keys = new KeyCode[1] { key };
    }
    public ControlItem(string description, KeyCode[] keys, MouseDirection direction)
    {
        this.description = description;
        this.direction = direction;
        this.keys = keys;
    }
    public ControlItem(string description, KeyCode[] keys, MouseButton button)
    {
        this.description = description;
        this.button = button;
        this.keys = keys;
    }
    public ControlItem(string description, KeyCode[] keys, MouseDirection direction, MouseButton button)
    {
        this.description = description;
        this.direction = direction;
        this.button = button;
        this.keys = keys;
    }
    public ControlItem(string description, CustomDisplay custom)
    {
        this.description = description;
        this.custom = custom;
    }

    public override string ToString()
    {
        string msg="";
        if (keys != null)
        {
            foreach(KeyCode key in keys)
                msg += string.Format("[{0}]", key);
            if(button != MouseButton.None)
                msg += " + ";
        }
        if (direction != MouseDirection.None)
            msg += string.Format("[dir {0}]", direction.ToString("G"));
        if(button != MouseButton.None)
            msg += string.Format("[but {0}]", button.ToString("G"));

        msg += string.Format(": {0}", description);
        return msg;
    }
}

public class ShowControls : MonoBehaviour {

    public GUISkin gui;

    public ArrayList controls;

    public bool destroyWhenDone = false;
    public float showDuration = 3;
    public bool hideLeftRightOnModifierKeys = true;
    public bool pauseOnDisplay = false;

    public ShowControlStyle style = ShowControlStyle.Dock;
    public KeyCode fullscreenClearKey = KeyCode.Tab;
    public string fullscreenTitle = "Controls";
    public int fullscreenTitleHeight = 100;
    public string fullscreenMessageLeft = "Press ";
    public string fullscreenMessageRight = " to continue";

    public ShowControlPosition position = ShowControlPosition.Top;

    public Texture keyBaseSmall;
    public Texture keyBaseLarge;

    public Texture mouseBase;
    public Texture mouseLeftClick;
    public Texture mouseMiddleClick;
    public Texture mouseRightClick;
    public Texture mouseWheel;
    public Texture mouseHorizontal;
    public Texture mouseVertical;
    public Texture mouseHorizontalAndVertical;
    public Texture plus;

    private const int texSize = 64;

    // offsets in the gui custom styles
    private static int TITLE_STYLE = 0;
    private static int KEYBOARD_STYLE = 1;
    // created dynamically based off title style
    private GUIStyle fullscreenBottomLeftStyle;
    private GUIStyle fullscreenBottomRightStyle;

    // make this configurable someday
    private static int NUM_COLUMNS = 2;

    private bool doShow = false;
    private float showStart = -1, showStop = -1, slideSpeed=.25f, savedTimeScale;

    public static ShowControls CreateDock(ArrayList controls, bool showOnCreate=true)
    {
        GameObject prefab = (GameObject)Resources.Load("DefaultShowControls");
        GameObject obj = (GameObject)Instantiate(prefab);
        ShowControls sc = obj.GetComponent<ShowControls>();
        sc.controls = controls;
        sc.destroyWhenDone = true;
        if (showOnCreate)
            sc.Show();
        return sc;
    }
    public static ShowControls CreateFullscreen(ArrayList controls, bool showOnCreate=true)
    {
        GameObject prefab = (GameObject)Resources.Load("DefaultShowControls");
        GameObject obj = (GameObject)Instantiate(prefab);
        ShowControls sc = obj.GetComponent<ShowControls>();
        sc.controls = controls;
        sc.style = ShowControlStyle.FullScreen;
        sc.slideSpeed = -1;
        sc.showDuration = -1;
        sc.pauseOnDisplay = true;
        sc.destroyWhenDone = true;
        if (showOnCreate)
            sc.Show();
        return sc;
    }
    public void Show()
    {
        if (doShow)
        {
            Debug.LogError(name + " already showing, ignoring second show request.");
            return;
        }
        doShow = true;
        showStart = Time.time;
        if (pauseOnDisplay)
        {
            savedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
        if (style == ShowControlStyle.FullScreen)
        {
            fullscreenBottomLeftStyle = new GUIStyle(gui.customStyles[TITLE_STYLE]);
            fullscreenBottomLeftStyle.alignment = TextAnchor.MiddleRight;
            fullscreenBottomRightStyle = new GUIStyle(gui.customStyles[TITLE_STYLE]);
            fullscreenBottomRightStyle.alignment = TextAnchor.MiddleLeft;
        }
    }
    /* Hide() indicates we want to stop showing.  Note that this doesn't
     * necessarily mean we stop showing immediately; if we're sliding,
     * we keep showing through the slide. */
    public void Hide()
    {
        if (style == ShowControlStyle.FullScreen || slideSpeed == -1)
        {
            Finished();
            return;
        }
        if (showStop != -1)
        {
            Debug.LogError("Already hiding, ignoring second hide request.");
            return;
        }
        showStop = Time.time;
    }
    /* Finished() is called when we are really done displaying,
     * either because we're instantaneously toggling off, or because
     * we're sliding and have finished the slide. */
    private void Finished()
    {
        doShow = false;
        if (pauseOnDisplay)
        {
            Time.timeScale = savedTimeScale;
            AudioListener.pause = false;
        }
        if (destroyWhenDone)
            Destroy(gameObject);
    }

    public void OnGUI()
    {
        if (!doShow)
            return;

        /* if we'd already started hiding, see if we're done */
        if (showStop != -1 && Time.time > showStop + slideSpeed)
        {
            Finished();
            return;
        }

        /* if we've got a max duration, haven't begun stopping yet, and
         * are past the time that we want to stop, hide. */ 
        if (showDuration != -1 && showStop == -1 && Time.time > showStart + showDuration)
        {
            Hide();
            // if we're sliding, we keep showing.  If not, we're done
            if (doShow)
                DisplayControls();
            return;
        }
        /* if we're fullscreen, check for the clear key */
        if (style == ShowControlStyle.FullScreen)
        {
            if (Input.GetKeyDown(fullscreenClearKey))
            {
                Hide();
                return;
            }

        }
        // normal case, just display.
        DisplayControls();
    }

    private void DisplayControls()
    {
        int x=0, y, slideOffset=0;
        if (style == ShowControlStyle.FullScreen)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
            GUI.Label(new Rect(0, 0, Screen.width, fullscreenTitleHeight), fullscreenTitle, gui.customStyles[TITLE_STYLE]);

            /* 3 things are used on the bottom - "Press " label on the left side,
             * widget for the clear key in the center, and " to continue" label
             * on the right side. */
            // TODO adjust width of bottom labels based on relative sizes
            int PADDING = 5;
                    
            int labelPos = Screen.height - fullscreenTitleHeight;
            int labelWidth = (Screen.width / 2) - (texSize / 2 + (2*PADDING));
            Rect r = new Rect(0, labelPos, labelWidth, fullscreenTitleHeight);
            GUI.Label(r, fullscreenMessageLeft, fullscreenBottomLeftStyle);

            int yPos = labelPos + (fullscreenTitleHeight - texSize) / 2;
            r = new Rect(labelWidth + PADDING, yPos, texSize, texSize);
            ShowKey(r, fullscreenClearKey);

            r = new Rect(labelWidth + texSize + PADDING * 2, labelPos, labelWidth, fullscreenTitleHeight);
            GUI.Label(r, fullscreenMessageRight, fullscreenBottomRightStyle);

            y = fullscreenTitleHeight;
        }
        else
        {
            if (Time.time < showStart + slideSpeed)
            {
                int slideTotal = texSize * Mathf.CeilToInt((float)controls.Count / NUM_COLUMNS);
                float slidePercent = (slideSpeed - (Time.time - showStart)) / (float)slideSpeed;
                slideOffset = (int)(slidePercent * slideTotal);
            }
            if (showStop != -1)
            {
                int slideTotal = texSize * Mathf.CeilToInt((float)controls.Count / NUM_COLUMNS);
                float slidePercent = (Time.time - showStop) / (float)slideSpeed;
                slideOffset = (int)(slidePercent * slideTotal);
            }

            if (position == ShowControlPosition.Top)
                y = 0 - slideOffset;
            else
                y = Screen.height - texSize + slideOffset;
        }

        ShowAllControls(x, y);
    }

    private void ShowAllControls(int x, int y)
    {
        bool shiftRight = false;
        foreach (ControlItem control in controls)
        {
            if (shiftRight)
                x = Screen.width / 2;
            else
                x = 0;

            ShowControl(control, x, y);

            if (shiftRight)
            {
                if (position == ShowControlPosition.Top)
                    y += texSize+5;
                else
                    y -= texSize+5;
                shiftRight = false;
            }
            else
            {
                shiftRight = true;
            }
        }
    }
    private void ShowControl(ControlItem control, int x, int y)
    {
        Rect texRect = new Rect(x, y, texSize, texSize);
        Rect labelRect;

        if (control.custom != null)
        {
            ShowCustom(texRect, control.custom);
            texRect.x += texSize;
        }
        // draw each of the keys
        if (control.keys != null)
        {
            foreach (KeyCode key in control.keys)
            {
                ShowKey(texRect, key);
                texRect.x += texSize;
            }
        } 

        // draw the mouse, if necessary
        if (control.button != MouseButton.None || control.direction != MouseDirection.None)
        {
            // if we already showed keys, also show a plus between keys & mouse
            if (control.keys != null)
            {
                Rect plusRect = new Rect(texRect.x, texRect.y + (texSize *.375f), texSize / 4, texSize / 4);
                GUI.DrawTexture(plusRect, plus);
                texRect.x += texSize / 4;
            }
            ShowMouse(texRect, control.direction, control.button);
            texRect.x += texSize;
        }
        // put the text description in the leftover space
        labelRect = new Rect(texRect.x, y, (Screen.width / 2) - (texRect.x - x), texSize);
        GUI.Box(labelRect, control.description, gui.box);
    }

    private void ShowCustom(Rect texRect, CustomDisplay custom)
    {
        GUI.DrawTexture(texRect, custom.customTexture);
    }

    private void ShowKey(Rect texRect, KeyCode key)
    {
        Texture tex = null;
        string label = null;
        if (ControlItem.BigKeys.ContainsKey(key))
        {
            tex = keyBaseLarge;
            label = ControlItem.BigKeys[key];
            if (label == null)
                label = key.ToString();
            else
            {
                if (hideLeftRightOnModifierKeys &&
                    (label.StartsWith("R ") || label.StartsWith("L ")))
                    label = label.Substring(2);
            }
        }
        else
        {
            tex = keyBaseSmall;
            if (ControlItem.SmallKeys.ContainsKey(key))
                label = ControlItem.SmallKeys[key];
            else
                label = key.ToString();
        }
        GUI.DrawTexture(texRect, tex);
        Rect labelRect = new Rect(texRect.x, texRect.y, texSize, texSize - 15);
        GUI.Label(labelRect, label, gui.customStyles[KEYBOARD_STYLE]);
    }
    private void ShowMouse(Rect texRect, MouseDirection direction, MouseButton button)
    {
        GUI.DrawTexture(texRect, mouseBase);
        switch (button)
        {
            case MouseButton.None:
                break;
            case MouseButton.LeftClick:
                GUI.DrawTexture(texRect, mouseLeftClick); break;
            case MouseButton.RightClick:
                GUI.DrawTexture(texRect, mouseRightClick); break;
            case MouseButton.BothClick:
                GUI.DrawTexture(texRect, mouseLeftClick);
                GUI.DrawTexture(texRect, mouseRightClick); break;
            case MouseButton.MiddleClick:
                GUI.DrawTexture(texRect, mouseMiddleClick); break;
            case MouseButton.ScrollWheel:
                GUI.DrawTexture(texRect, mouseWheel); break;
            default:
                Debug.LogError("Unsupported MouseButton " + button);
                return;
        }
        switch (direction)
        {
            case MouseDirection.None:
                break;
            case MouseDirection.Horizontal:
                GUI.DrawTexture(texRect, mouseHorizontal); break;
            case MouseDirection.Vertical:
                GUI.DrawTexture(texRect, mouseVertical); break;
            case MouseDirection.Both:
                GUI.DrawTexture(texRect, mouseHorizontalAndVertical); break;
            default:
                Debug.LogError("Unsupported MouseDirection " + direction);
                return;
        }
    }
}