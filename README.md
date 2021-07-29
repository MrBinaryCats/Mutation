# Mutation
Mutation system for Unity which allows you to adapt your objects via data. The project provides an `Entity` data type which implements the Mutation system. Each `Entity` can have different mutations. Provided Mutations include:
- Rotate Mutation
- Color Mutation
- Scale Mutation
- Strength Mutation
Mutations are saved as subassets of the entity. 

The project also includes a component which uses the entity data and the mutations to mutate the gameobject, see EntityController.cs.

<img width="603" alt="Screenshot 2021-07-29 at 20 43 59" src="https://user-images.githubusercontent.com/85991229/127555963-feb4440c-f8a3-40d6-ba4f-8ee003bbc49b.png">

## Limitations
- Currently, by design, you can only have one mutation of each type on an entity
- To create a new mutation of inheriting from `Mutations` you must create a (dummy) custom editor (see code examples)
- To see the created mutations in the project window, you must save the project

## Using Mutation
### Create an Enity
To create an entity
1. `Assets>Create>Entity`
2. Name your Entity Data

### Add Mutations to your Entity
To add a Mutation to your entity
1. Select your your entity data
2. Click `Add Mutation` in the inspector window
3. Select which Mutation you would like to add
4. Edit the Mutations values
5. Save the project

### Remove Mutations to your Entity
1. Select your your entity data
2. Expand the mutation you wish to remove
3. Click `Remove this Mutation`
4. Save the project

## Code Examples

### Getting a Mutation from EntityData
```cs
if (data.TryGetMutation<RotateMutation>(out var rotMutation))
  rotMutation.ResetToDefault(GetComponent<Rotator>());
 ```
### Iterating Mutation States
```cs
data.TryGetMutation<ColorMutation>(out _colorMutator)
...
//Store the next index so it can be passed in and used again
_colorIndex = _colorMutator.ApplyNext(_meshRenderer, _colorIndex);
```
### Defining (Dummy) Editors for Mutations
```cs
//Declare this is a custom editor for the color mutation
[CustomEditor(typeof(typeof(ColorMutation), true)]
//inherit from MutationsEditor which will do all the work for you
public class ColorEditor : MutationsEditor{};
```
