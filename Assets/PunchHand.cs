using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class PunchHand : MonoBehaviour
{

    public SteamVR_TrackedObject hand;
    private Rigidbody rBody;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rBody.MovePosition(hand.transform.position);
        rBody.MoveRotation(hand.transform.rotation);
    }

    void OnCollisionEnter(Collision other)
    {
        UnityEngine.Debug.Log("Hit");

        Rigidbody otherR = other.gameObject.GetComponentInChildren<Rigidbody>();
        if (other == null)
        {
            return;
        }


        Vector3 avgPoint = Vector3.zero;

        foreach(ContactPoint p in other.contacts)
        {
            avgPoint += p.point;
        }

        avgPoint /= other.contacts.Length;

        Vector3 dir = (avgPoint - transform.position).normalized;
        otherR.AddForceAtPosition(dir * 10f * rBody.velocity.magnitude, avgPoint);

        StartCoroutine( LongVibration(0.1f, 0.2f) );
    }

    IEnumerator LongVibration(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            SteamVR_Controller.Input((int)hand.index).TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
        }

        yield return null;
    }

    IEnumerator LongVibration(int vibrationCount, float vibrationLength, float gapLength, float strength)
    {
        strength = Mathf.Clamp01(strength);
        for (int i = 0; i < vibrationCount; i++)
        {
            if (i != 0)
            {
                yield return new WaitForSeconds(gapLength);
            }

            yield return StartCoroutine(LongVibration(vibrationLength, strength));
        }
    }
}
