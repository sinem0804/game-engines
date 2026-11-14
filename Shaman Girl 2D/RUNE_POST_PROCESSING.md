# Rune collection post-processing

## Unity setup (outside Play Mode)

1. Open `Assets/Scenes/Level1_ForgottenVillage_New.unity`.
2. Select **Main Camera** (keep its **MainCamera** tag). Add the **Rune Collection Post Process** component. Leave Magical Tint and Strength at their defaults initially.
3. In the camera's URP **Rendering** settings, enable **Post Processing**. This is currently disabled in the saved scene.
4. Keep the camera on the **Default** layer and ensure its **Volumes > Volume Mask** includes **Default** (the current mask already does). The runtime Volume inherits the camera GameObject's layer.
5. Set **Volume Update Mode** to **Every Frame** if exposed, so the animated Volume is evaluated continuously. Keep the camera's Renderer as **Use Pipeline Settings**: the assigned `UniversalRP` asset uses `Renderer2D`.
6. Save the gameplay scene.

No Volume Profile asset or Global Volume needs to be created manually. The component creates a child Global Volume and a private profile containing only a Color Adjustments / Color Filter override at runtime. Priority is 100; weight starts at zero. Existing Volume Profiles, including `Assets/Settings/DefaultVolumeProfile.asset`, are never edited. Existing post-processing values continue to blend normally; only Color Filter is temporarily blended toward the magical tint. Zero weight restores the original Volume result.

The installed URP version is 17.6.0, with Unity 6000.6.4f1. Every quality level selects `Assets/Settings/UniversalRP.asset`; Graphics Settings alone has no default pipeline assigned, but these quality overrides select URP. `Renderer2D.asset` already references post-processing resources, so no renderer feature, shader, HDR, bloom, depth texture, or opaque texture is required. If adding other Volumes later, keep their priority below 100 for this tint to take precedence.

## Test and demonstrate

- Enter Play Mode. Before collecting anything, the screen should have its normal colors.
- Collect each of the four Spiral Runes: the game world should gain a subtle blue-purple tint, reaching its peak in 0.08 seconds and smoothly returning to normal by 0.5 seconds.
- Check that each rune still adds its normal points, updates the rune count, plays particles, and disappears. The fourth rune must still unlock Magic Leap.
- Select Main Camera and its generated **Rune Collection Volume (Runtime)** child. Watch Volume Weight rise to at most 0.45 and return to exactly zero. Its profile has only the Color Filter override enabled.
- For a repeatable close-pickup test, use the camera component's context menu **Test Rune Pulse (Play Mode)** several times quickly. Also collect two runes quickly when practical. The pulse restarts from its current weight, remains bounded, and ends half a second after the last trigger.
- Disable the camera's Post Processing checkbox temporarily in Play Mode and trigger the test again: the tint disappears. Re-enable it to demonstrate that this is URP post-processing, driven dynamically by the collection event. A Screen Space Overlay UI is rendered separately and can retain its normal colors.
- Stop Play Mode. The generated Volume/profile are temporary; existing profile assets remain unchanged. Runtime camera checkbox changes are not saved, so ensure Post Processing is enabled outside Play Mode as described above.

For the assignment: "Collecting a rune triggers a script that animates the weight of a URP Global Volume containing Color Adjustments. A smooth attack and fade change the rendered screen color for half a second, then restore the existing post-processing result."

## Files

- Created `Assets/Scripts/RuneCollectionPostProcess.cs` and its Unity metadata.
- Modified `Assets/Scripts/SpiralRuneCollectible.cs`: one optional call after awarding points triggers the camera component.
- Created this guide. `GameManager.cs`, movement, animation, particles, menus, scenes, and existing Volume Profiles were not edited.

Implementation was checked against the installed URP/core package source. Unity compilation and visual gameplay verification must be performed in the Editor after the setup above.
