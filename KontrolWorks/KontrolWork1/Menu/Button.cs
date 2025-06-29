namespace KontrolWork1.Menu;

public abstract class Button
{
    // ☐ ☑ ☑ ○ ✅ ❎ 🔘 🔲 🔵 🔳 ❶ ❷ ❸ ❹ ❺ ❻ ❼ ❽ ❾ ❿ ① ② ③ ④ ⑤ ⑥ ⑦ ⑧ ⑨ ⑩

    public abstract string IconOff { get; }
    public abstract string[] IconOn { get; }
    public abstract string Text { get; set; }
    public abstract string[] Colors { get; }
}