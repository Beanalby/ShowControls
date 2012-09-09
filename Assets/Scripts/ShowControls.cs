using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MouseDirection { None, Horizontal, Vertical, Both }
public enum MouseButton { None, LeftClick, RightClick, BothClick, MiddleClick, ScrollWheel };

public class Control
{
    public string description;
    public KeyCode[] keys = null;
    public MouseButton button = MouseButton.None;
    public MouseDirection direction = MouseDirection.None;

    public Control(string description, KeyCode key)
    {
        this.description = description;
        this.keys = new KeyCode[1] { key };
    }
    public Control(string description, KeyCode[] keys)
    {
        this.description = description;
        this.keys = keys;
    }
    public Control(string description, MouseDirection direction)
    {
        this.description = description;
        this.direction = direction;
    }
    public Control(string description, MouseButton button)
    {
        this.description = description;
        this.button = button;
    }
    public Control(string description, MouseDirection direction, MouseButton button)
    {
        this.description = description;
        this.direction = direction;
        this.button = button;
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

[ExecuteInEditMode]
public class ShowControls : MonoBehaviour {

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

    public GUISkin gui;
    public float showDuration = 5;

    public Texture keyBaseSmall;
    public Texture keyBaseLarge;
    public GUIStyle keyStyle;

    public Texture mouseBase;
    public Texture mouseLeftClick;
    public Texture mouseMiddleClick;
    public Texture mouseRightClick;
    public Texture mouseWheel;
    public Texture mouseHorizontal;
    public Texture mouseVertical;
    public Texture mouseHorizontalAndVertical;

    private const int texSize = 64;

    public ArrayList controls;
    private bool doShow = false;
    private float showStart = -1;

	// Use this for initialization
	void Start () {
        keyStyle.alignment = TextAnchor.MiddleCenter;
        keyStyle.fontStyle = FontStyle.Bold;
	}

    public void Show()
    {
        doShow = true;
        showStart = Time.time;
    }

    public void OnGUI()
    {
        if (gui != null)
            GUI.skin = gui;
        Control mega = new Control("COMBO!", MouseButton.RightClick);
        mega.keys = new KeyCode[] { KeyCode.LeftApple };
        controls = new ArrayList(new[] {
            new Control("oneKey", KeyCode.Insert),
            new Control("manyKey", new KeyCode[] { KeyCode.RightApple, KeyCode.LeftShift}),
            new Control("oneDir", MouseDirection.Horizontal),
            new Control("oneButton", MouseButton.LeftClick),
            mega,
            new Control("manyButton", MouseButton.BothClick),
            new Control("oneDir+oneButton", MouseButton.ScrollWheel),
            new Control("oneDir+manyButton", MouseDirection.Both, MouseButton.MiddleClick)
        });

        bool shiftRight = false;
        int x =0, y = 0;

        foreach (Control control in controls)
        {
            if (shiftRight)
                x = Screen.width / 2;
            else
                x = 0;

            ShowControl(control, x, y);

            if (shiftRight)
            {
                y += 80;
                shiftRight = false;
            }
            else
            {
                shiftRight = true;
            }
        }
    }
    private void ShowControl(Control control, int x, int y)
    {
        int widgetsShown = 0;
        Rect texRect = new Rect(x, y, texSize, texSize);
        Rect labelRect;

        // draw each of the keys
        if (control.keys != null)
        {
            foreach (KeyCode key in control.keys)
            {
                Texture tex = null;
                string label = null;
                if(BigKeys.ContainsKey(key))
                {
                    tex = keyBaseLarge;
                    label = BigKeys[key];
                    if(label == null)
                        label = key.ToString();
                }
                else
                {
                    tex = keyBaseSmall;
                    if (SmallKeys.ContainsKey(key))
                        label = SmallKeys[key];
                    else
                        label = key.ToString();
                }       
                GUI.DrawTexture(texRect, tex);
                labelRect = new Rect(texRect.x, texRect.y, texSize, texSize - 15);
                GUI.Label(labelRect, label, keyStyle);
                texRect.x += texSize;
                widgetsShown++;
            }
        } 

        // draw the mouse, if necessary
        if (control.button != MouseButton.None || control.direction != MouseDirection.None)
        {
            GUI.DrawTexture(texRect, mouseBase);
            switch (control.button)
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
                    Debug.LogError("Unsupported MouseButton " + control.button);
                    return;
            }
            switch (control.direction)
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
                    Debug.LogError("Unsupported MouseDirection " + control.direction);
                    return;
            }
            texRect.x += texSize;
            widgetsShown++;
        }

        // put the text description in the leftover space
        int offset = (widgetsShown * texSize);
        labelRect = new Rect(x + offset, y, (Screen.width / 2) - offset, texSize);
        GUI.Box(labelRect, control.ToString());
    }
}