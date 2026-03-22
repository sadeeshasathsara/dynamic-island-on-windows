namespace DynamicIslandNotifier.UI;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

/// <summary>
/// Encapsulates all WPF animations for the Dynamic Island pill:
/// slide, resize, fade, corner-radius interpolation, expand/collapse orchestration.
/// </summary>
public sealed class IslandAnimator
{
    // ── Size Constants ─────────────────────────────────────────────────
    public const double PILL_W_OPEN      = 520;
    public const double PILL_H_OPEN      = 64;
    public const double PILL_H_OPEN_BODY = 84;
    public const double PILL_H_HOVER     = 120;

    private readonly Border _pill;
    private readonly TranslateTransform _pillTransform;
    private readonly UIElement _contentRoot;
    private readonly UIElement _hoverPanel;

    public IslandAnimator(Border pill, TranslateTransform pillTransform,
                          UIElement contentRoot, UIElement hoverPanel)
    {
        _pill          = pill;
        _pillTransform = pillTransform;
        _contentRoot   = contentRoot;
        _hoverPanel    = hoverPanel;

        // Dynamic corner-radius: morphs from pill → rounded box on hover expansion
        _pill.SizeChanged += (_, _) =>
        {
            double h = _pill.ActualHeight;
            double r = h / 2;
            if (h > 84)
            {
                double progress = (h - 84) / (120.0 - 84.0);
                r = 42.0 - (progress * (42.0 - 24.0));
            }
            _pill.CornerRadius = new CornerRadius(r);
        };
    }

    // ── Expand / Collapse Orchestration ───────────────────────────────
    public void Expand(bool hasBody, Action onFullyOpen)
    {
        _pill.Width  = 64;
        _pill.Height = 64;
        _hoverPanel.Opacity    = 0;
        _hoverPanel.Visibility = Visibility.Collapsed;
        _contentRoot.Opacity   = 0;
        _pillTransform.Y       = -150;

        SlideY(-150, 0, 450, new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.5 }, () =>
        {
            double targetH = hasBody ? PILL_H_OPEN_BODY : PILL_H_OPEN;
            SizePill(PILL_W_OPEN, targetH, 350,
                new CubicEase { EasingMode = EasingMode.EaseOut }, onDone: onFullyOpen);
            FadeIn(_contentRoot, 250, delay: 100);
        });
    }

    public void Collapse(Action onHidden)
    {
        FadeOut(_contentRoot, 150, () =>
        {
            SizePill(64, 64, 300, new CubicEase { EasingMode = EasingMode.EaseIn }, onDone: () =>
            {
                SlideY(0, -150, 300, new CubicEase { EasingMode = EasingMode.EaseIn },
                    onDone: onHidden);
            });
        });
    }

    public void ExpandToHover()
    {
        SizePill(PILL_W_OPEN, PILL_H_HOVER, 250,
            new CubicEase { EasingMode = EasingMode.EaseOut });
        FadeIn(_hoverPanel, 200, delay: 100);
    }

    public void CollapseFromHover(bool hasBody)
    {
        FadeOut(_hoverPanel, 150);
        double targetH = hasBody ? PILL_H_OPEN_BODY : PILL_H_OPEN;
        SizePill(PILL_W_OPEN, targetH, 250,
            new CubicEase { EasingMode = EasingMode.EaseIn });
    }

    // ── Primitives ────────────────────────────────────────────────────
    public void SlideY(double from, double to, int ms, IEasingFunction ease, Action? onDone = null)
    {
        var anim = new DoubleAnimation(from, to, TimeSpan.FromMilliseconds(ms)) { EasingFunction = ease };
        if (onDone != null) anim.Completed += (_, _) => _pill.Dispatcher.Invoke(onDone);
        _pillTransform.BeginAnimation(TranslateTransform.YProperty, anim);
    }

    public void SizePill(double toW, double toH, int ms, IEasingFunction ease, Action? onDone = null)
    {
        var dur   = TimeSpan.FromMilliseconds(ms);
        var wAnim = new DoubleAnimation(_pill.Width,  toW, dur) { EasingFunction = ease };
        var hAnim = new DoubleAnimation(_pill.Height, toH, dur) { EasingFunction = ease };
        if (onDone != null) hAnim.Completed += (_, _) => _pill.Dispatcher.Invoke(onDone);
        _pill.BeginAnimation(FrameworkElement.WidthProperty,  wAnim);
        _pill.BeginAnimation(FrameworkElement.HeightProperty, hAnim);
    }

    public static void FadeIn(UIElement el, int ms, int delay = 0)
    {
        el.Visibility = Visibility.Visible;
        var a = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(ms))
            { BeginTime = TimeSpan.FromMilliseconds(delay) };
        el.BeginAnimation(UIElement.OpacityProperty, a);
    }

    public void FadeOut(UIElement el, int ms, Action? done = null)
    {
        var a = new DoubleAnimation(el.Opacity, 0, TimeSpan.FromMilliseconds(ms));
        a.Completed += (_, _) => { el.Visibility = Visibility.Collapsed; done?.Invoke(); };
        el.BeginAnimation(UIElement.OpacityProperty, a);
    }
}
