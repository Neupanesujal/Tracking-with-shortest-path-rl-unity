using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MovNShoot : Agent
 {
    [SerializeField] private Transform target;
    public float moveSpeed= 4f;
    float rayLength = 30f; 
    float[] rayAngles = { 0f, 30f,60f, 90f, 120f, 150f, 180f ,210f, 240f, 270f, 300f, 330f};
    //string[] detectableObjects = { "GoalTag" };
    //List<int> rayAngles = new List<int> {0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330};
    RaycastHit[] rayHits = new RaycastHit[12];

    //List<float> rayAngles = new List<float> {0f, 0.5236f, 1.0472f, 1.5708f, 2.0944f, 2.6179f, 3.1416f, 3.6652f, 4.1888f, 4.7124f, 5.2360f, 5.7596f};
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material looseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;

    float shotAvailable= 25f;
    int shootAction;

    private void Update() 
    {
        RayPerception();
        shotAvailable--;
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition= new Vector3(Random.Range(-7, 7),0f, Random.Range(-4, 12));
        target.localPosition= new Vector3(Random.Range(-7, 7),0f, Random.Range(-4, 12));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        shootAction = actions.DiscreteActions[0];
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        //float shootAction= actions.ContinuousActions[2];
        
        // if (shootAction == 0)
        // {
        //     Shoot();
        // }
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    //bool isReleased = true, canPress = true;
    public override void Heuristic(in ActionBuffers actionOut)
    {
        ActionSegment<float> continuousActions= actionOut.ContinuousActions;
        ActionSegment<int> discreteActions= actionOut.DiscreteActions;
        discreteActions[0]= Input.GetKey(KeyCode.Space) ? 0 : 1;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
        
        //continuousActions[2] = Input.GetKeyDown(KeyCode.Space) ? 0f : 1f;
    }
    
    private void RayPerception()
    {
        for(int i=0; i < rayAngles.Length; i++)
        {
            Vector3 rayDirection = Quaternion.Euler(0f, rayAngles[i], 0f) * transform.forward;
            Debug.DrawRay(transform.position, rayDirection, Color.green, 100f);
            if (Physics.Raycast(transform.position, rayDirection,out RaycastHit hit, rayLength))
            {
                if (hit.collider.CompareTag("goal"))
                {
                    transform.Rotate(0f,30f*i,0f);
                    if(shootAction == 0)
                    {
                        Shoot();
                    }
                    
                }
            }
        }
    }

    void Shoot()
    {
        if(shotAvailable > 0f)
        {
            return;
        }
        shotAvailable= 25f;
        Debug.Log("PlayerShooted");
         if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayLength))
            {
             if (hit.collider.CompareTag("goal"))
              {
                Debug.Log("Goal got hit");
                floorMeshRenderer.material= winMaterial;
                SetReward(+1f);
                EndEpisode();
            }
            }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("wall"))
        {
            Debug.Log("Player le wall xoyo");
            SetReward(-1f);
            floorMeshRenderer.material= looseMaterial;
            EndEpisode();
        }
    }
}
