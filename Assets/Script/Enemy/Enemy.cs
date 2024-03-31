using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Assets.Script
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int HP = 100;
        private Animator animator;

        private NavMeshAgent navAgent;
        private CapsuleCollider capsuleCollider;

        public bool isDead;
        void Start()
        {
            animator = GetComponent<Animator>();
            navAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        public void TakeDamage(int damageAmount)
        {
            HP -= damageAmount;

            if (HP <= 0)
            {
                int randomValue = Random.Range(0, 2);

                if (randomValue == 0)
                {
                    animator.SetTrigger("DIE1");
                }
                else
                {
                    animator.SetTrigger("DIE2");
                }

                isDead = true;

                SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieDeath);

                StartCoroutine(DeleteEnemy());

            }
            else
            {
                animator.SetTrigger("DAMAGE");

                SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHurt);

            }
        }

        private IEnumerator DeleteEnemy()
        {
            yield return new WaitForSeconds(1f);
            GameObject.Destroy(GetComponent<CapsuleCollider>());

            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(transform.position, 1.5f); //Attacking //Stop attacking

            //Gizmos.color = Color.blue;
            //Gizmos.DrawWireSphere(transform.position, 10f); //Detecion (Start chasing)

            //Gizmos.color = Color.green;
            //Gizmos.DrawWireSphere(transform.position, 12f); //Stop chasing
        }
    }

    
}
