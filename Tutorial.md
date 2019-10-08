# Vision tutorial

This shows how to make simple view cone detection.

## 1. Create a new scene

Start by creating a new scene called `Vision`.

Add a 3D Cube named `Guard`, and a 3D Cube named `Player`.

Make a new `Blue` material and drag it onto `Guard`, then make a new `Green` material and drag it onto the `Player`.

## 2. Add the vision cone

Create a new script called `Vision`.

We're going to make the detection out of two pieces: a sphere collider trigger to detect the player entering the view radius, and an angle calculation to detect when the player is within the viewing angle. To enfore the requirement of a SphereCollider, add a `RequireComponent` instruction above the class definition and add a variable to hold the `SphereCollider` called `viewDistance`:
```
[RequireComponent(typeof(SphereCollider))]
public class Vision : MonoBehaviour
{
    public SphereCollider viewDistance;
}
```

Now add the Vision script to the `Guard` object. This should automatically add a `SphereCollider` as well. Set the `SphereCollider` radius to 10 and tick the `Is Trigger` box. Drag the collider component onto the `viewDistance` slot of the `Vision` behaviour.

We could add the trigger detection behaviour right away, but it would be nice to be able to see the view cone at least within the editor. To do this add a variable to hold the field of view and a function called `OnDrawGizmos()`:
```
    public float fov = 90.0f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawFrustum(Vector3.zero, fov, viewDistance.radius, 0.5f, 1.0f);
        Gizmos.DrawWireSphere(Vector3.zero, viewDistance.radius);
    }
```

Now switch back to the Unity editor. You should see red lines showing both the view distance and the angles of the field of view. It's easier to see if you use a top down orthographic view.

![top down view of the guard object showing view radius and field of view](https://raw.githubusercontent.com/LSBUGPG/Vision/master/ViewCone.png "Top down scene view of the `Guard` object")

You can change the view distance by changing the `SphereCollision`'s `radius` in the inspector window. And you can change the field of view by changing the `fov` parameter in the `Vision` script.

## 3. Detect the player

Change the tag of the `Player` object to `Player` and add a `Rigidbody` component with `Use Gravity` off and `Is Kinematic` on. The `Rigidbody` component is needed to cause collision detection, and the tag is required to identify the player object. With these in place, you can add the trigger code to the `Vision` script:
```
    void OnTriggerStay(Collider thing)
    {
        if (thing.CompareTag("Player"))
        {
            Vector3 vectorToPlayer = thing.transform.position - transform.position;
            if (Vector3.Angle(vectorToPlayer, transform.forward) < 0.5f * fov)
            {
                Debug.Log("Alert");
            }
        }
    }
```

Test this out by running the game and dragging the player object into the range of the view cone. You should see the message `Alert` appear in the console window.

## 4. React to alerts

You'll need to create an alert sound effect and at it to your assets. Add an `AudioSource` component, untick the `Play On Awake` option, and assign your sound effect to the `AudioClip` slot. Also, create a `Red` material.

Create a new script called `Alert` and enter the following code:
```
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Renderer))]
public class Alert : MonoBehaviour
{
    public Material alertedMaterial;

    void OnEnable()
    {
        Renderer boxRenderer = GetComponent<Renderer>();
        boxRenderer.material = alertedMaterial;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}
```

Add this script to your `Guard` object, but untick the box next to the component. Drag the `Red` material onto the `Alerted Material` slot.

You can test this alert works, by running the game and ticking the box next to the component to enable it.

## 5. Combining the components

In the `Vision` script add a variable to hold the alert script:
```
    public Behaviour alert;
```

Switch to the Unity editor and drag the alert behaviour onto the `alert` slot of the `Vision` behaviour. And finally, change the line:
```
    Debug.Log("Alert");
```
to:
```
    alert.enabled = true;
```

Test by running the game and dragging the player object into the view cone of the guard.
