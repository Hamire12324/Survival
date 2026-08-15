using System;

/// <summary>
/// Compatibility alias for scenes created before progression was consolidated
/// into <see cref="HeroLevel"/>. New content should use HeroLevel directly.
/// </summary>
[Obsolete("Use HeroLevel instead. This compatibility component has no separate progression logic.")]
public sealed class HeroProgression : HeroLevel
{
}
