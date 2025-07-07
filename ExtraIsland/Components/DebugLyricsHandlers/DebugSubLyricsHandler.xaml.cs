﻿using ClassIsland.Core.Attributes;
using ExtraIsland.Shared;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;

namespace ExtraIsland.Components;

// TODO: 调试结束后移除此组件
[ComponentInfo(
    "D61B565D-5BC9-9999-8888-2EDB22F9756E",
    "调试 · 副歌词",
    PackIconKind.Bug,
    "继续测试歌词岛封装类LyricsIslandHandler()"
)]
public partial class DebugSubLyricsHandler {
    public DebugSubLyricsHandler() {
        if (EiUtils.IsPluginInstalled("ink.lipoly.ext.lychee")) {
            _handler = new LycheeLyricsProvider();
        } else {
            GlobalConstants.Handlers.LyricsIsland ??= new LyricsIslandLyricsProvider();
            _handler = GlobalConstants.Handlers.LyricsIsland;   
        }
        InitializeComponent();
        _handler.OnLyricsChanged += UpdateLyrics;
        _animator = new Animators.ClockTransformControlAnimator(LyricsLabel,-0.3);
        new Thread(CounterDaemon).Start();
    }

    readonly ILyricsProvider _handler;
    readonly Animators.ClockTransformControlAnimator _animator;

    int _timeCounter = 10;
    
    void UpdateLyrics() {
        this.BeginInvoke(() => {
            _timeCounter = 10;
            _animator.Update(_handler.SubLyrics, isForced:true);
        });
    }

    void CounterDaemon() {
        while (true) {
            _timeCounter -= 1;
            if (_timeCounter <= 0 & _animator.TargetContent != string.Empty) {
                this.BeginInvoke(() => {
                    _animator.Update(string.Empty);
                });
            }
            Thread.Sleep(1000);
        }
        // ReSharper disable once FunctionNeverReturns
    }
}