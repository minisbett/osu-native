using osu.Game.Online.API;
using osu.Native.Compiler;
using osu.Native.Structures;

namespace osu.Native.Objects;

/// <summary>
/// Represents a collection of mods (<see cref="List{T}"/> of <see cref="APIMod"/>).
/// </summary>
internal unsafe partial class ModsCollectionObject : IOsuNativeObject<ModsCollection>
{
    /// <summary>
    /// Creates an instance of a <see cref="List{APIMod}"/>.
    /// </summary>
    /// <param name="nativeModsCollectionPtr">A pointer to write the resulting native mods collection object to.</param>
    [OsuNativeFunction]
    public static ErrorCode Create(NativeModsCollection* nativeModsCollectionPtr)
    {
        *nativeModsCollectionPtr = new NativeModsCollection { Handle = ManagedObjectStore.Store(new ModsCollection()) };

        return ErrorCode.Success;
    }

    /// <summary>
    /// Adds the specified mod to the specified mods collection.
    /// </summary>
    /// <param name="modsCollectionHandle">The handle of the mods collection.</param>
    /// <param name="modHandle">The handle of the mod to add.</param>
    [OsuNativeFunction]
    public static ErrorCode Add(ModsCollectionHandle modsCollectionHandle, ManagedObjectHandle<APIMod> modHandle)
    {
        ModsCollection mods = modsCollectionHandle.Resolve();
        APIMod mod = modHandle.Resolve();

        mods.Add(mod);

        return ErrorCode.Success;
    }


    /// <summary>
    /// Removes the specified mod from the specified mods collection.
    /// </summary>
    /// <param name="modsCollectionHandle">The handle of the mods collection.</param>
    /// <param name="modHandle">The handle of the mod to remove.</param>
    [OsuNativeFunction]
    public static ErrorCode Remove(ModsCollectionHandle modsCollectionHandle, ManagedObjectHandle<APIMod> modHandle)
    {
        ModsCollection mods = modsCollectionHandle.Resolve();
        APIMod mod = modHandle.Resolve();

        mods.Remove(mod);

        return ErrorCode.Success;
    }

    [OsuNativeFunction]
    public static ErrorCode Debug(ModsCollectionHandle modsCollectionHandle)
    {
        ModsCollection mods = modsCollectionHandle.Resolve();

        Console.WriteLine($"ModsCollection({modsCollectionHandle.Id}):");
        foreach (APIMod mod in mods)
        {
            Console.WriteLine($"  Mod: {mod.Acronym}");
            foreach (KeyValuePair<string, object> setting in mod.Settings)
                Console.WriteLine($"    {setting.Key}: {setting.Value}");
        }

        return ErrorCode.Success;
    }
}