using UnityEngine;
using System.Collections;

public enum MouseDirection { None, Horizontal, Vertical, Both }
public enum MouseButton { None, LeftClick, RightClick, MiddleClick, WheelUp, WheelDown };

public class Control
{
    public string description;
    public KeyCode[] keys = null;
    public MouseButton[] buttons = null;
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
        if (button == MouseButton.None)
            this.buttons = null;
        else
            this.buttons = new MouseButton[] { button };
    }
    public Control(string description, MouseButton[] buttons)
    {
        this.description = description;
        this.buttons = buttons;
    }
    public Control(string description, MouseDirection direction, MouseButton button)
    {
        this.description = description;
        this.direction = direction;
        if(button == MouseButton.None)
            this.buttons = null;
        else
            this.buttons = new MouseButton[1] { button };
    }
    public Control(string description, MouseDirection direction, MouseButton[] buttons)
    {
        this.description = description;
        this.direction = direction;
        this.buttons = buttons;
    }

    public override string ToString()
    {
        string msg="";
        if (keys != null)
        {
            foreach(KeyCode key in keys)
                msg += string.Format("[{0}]", key);
            if(buttons != null || direction != MouseDirection.None)
                msg += " + ";
        }
        if (direction != MouseDirection.None)
            msg += string.Format("[dir {0}]", direction.ToString("G"));
        if(buttons != null)
            foreach(MouseButton button in buttons)
                msg += string.Format("[but {0}]", button.ToString("G"));

        msg += string.Format(": {0}", description);
        return msg;
    }
}

[ExecuteInEditMode]
public class ShowControls : MonoBehaviour {

    public GUISkin gui;
    public float showDuration = 5;

    public Texture keyBaseSmall;
    public Texture keyBaseLarge;
    public GUIStyle keyStyle;

    public Texture mouseBase;
    public Texture mouseOutline;
    public Texture mouseLeftClick;
    public Texture mouseMiddleClick;
    public Texture mouseRightClick;
    public Texture mouseWheel;
    public Texture mouseHorizontal;
    public Texture mouseVertical;

    private const int texSize = 80;

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
        Control mega = new Control("COMBO!", MouseButton.LeftClick);
        mega.keys = new KeyCode[] { KeyCode.LeftControl };
        controls = new ArrayList(new[] {
            new Control("oneKey", KeyCode.Space),
            new Control("manyKey", new KeyCode[] { KeyCode.LeftControl, KeyCode.F }),
            new Control("oneDir", MouseDirection.Horizontal),
            new Control("oneButton", MouseButton.LeftClick),
            mega,
            new Control("manyButton", new MouseButton[] { MouseButton.LeftClick, MouseButton.RightClick }),
            new Control("oneDir+oneButton", MouseDirection.Vertical, MouseButton.MiddleClick),
            new Control("oneDir+manyButton", MouseDirection.Both, new MouseButton[] { MouseButton.LeftClick, MouseButton.RightClick })
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
                GUI.DrawTexture(texRect, keyBaseSmall);
                labelRect = new Rect(texRect.x, texRect.y, texSize, texSize);
                GUI.Label(labelRect, key.ToString(), keyStyle);
                texRect.x += texSize;
                widgetsShown++;
            }
        }

        // draw the mouse, if necessary
        if (control.buttons != null || control.direction != MouseDirection.None)
        {
            GUI.DrawTexture(texRect, mouseBase);
            if (control.buttons != null)
            {
                foreach (MouseButton button in control.buttons)
                {
                    switch (button)
                    {
                        case MouseButton.LeftClick:
                            GUI.DrawTexture(texRect, mouseLeftClick); break;
                        case MouseButton.RightClick:
                            GUI.DrawTexture(texRect, mouseRightClick); break;
                        case MouseButton.MiddleClick:
                            GUI.DrawTexture(texRect, mouseWheel); break;
                        case MouseButton.WheelDown:
                            GUI.DrawTexture(texRect, mouseWheel); break;
                        case MouseButton.WheelUp:
                            GUI.DrawTexture(texRect, mouseWheel); break;
                        default:
                            Debug.LogError("Unsupported MouseButton " + button);
                            return;
                    }
                }
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
                    GUI.DrawTexture(texRect, mouseHorizontal);
                    GUI.DrawTexture(texRect, mouseVertical); break;
                default:
                    Debug.LogError("Unsupported MouseDirection " + control.direction);
                    return;
            }
            GUI.DrawTexture(texRect, mouseOutline);
            texRect.x += texSize;
            widgetsShown++;
        }

        // put the text description in the leftover space
        int offset = (widgetsShown * texSize);
        labelRect = new Rect(x + offset, y, (Screen.width / 2) - offset, texSize);
        GUI.Box(labelRect, control.ToString());
    }
}