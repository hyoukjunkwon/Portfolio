using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Missile : MonoBehaviour
{
	Rigidbody m_rigid = null;
	Transform m_tfTarget = null;

	[SerializeField]
	float m_speed = 0f;
	float m_currentSpeed = 0f;
	[SerializeField] LayerMask m_layerMask = 0;
	[SerializeField] ParticleSystem smoke = null;
	[SerializeField] ParticleSystem flame = null;
	private int damage = 10;

	void SearchEnemy()
	{ 
		Collider[] t_cols = Physics.OverlapSphere(transform.position, 100f, m_layerMask);
		if (t_cols.Length > 0)
		{
			m_tfTarget = t_cols[0].transform;
		
		}
	}

	IEnumerator LaunchDelay()
	{
		yield return new WaitUntil(() => m_rigid.velocity.y < 0.8f);
		yield return new WaitForSeconds(0.1f);

		SearchEnemy();
		smoke.Play();
		flame.Play();

		yield return new WaitForSeconds(5f);
		Destroy(gameObject);
	}
	void Start()
	{
		m_rigid = GetComponent<Rigidbody>();
        StartCoroutine(LaunchDelay());
		m_speed = Random.Range(25f, 50f);
	}

	void Update()
	{
		if (m_tfTarget != null)
		{
			if (m_currentSpeed <= m_speed)
				m_currentSpeed += m_speed * Time.deltaTime;
			transform.position += transform.up * m_currentSpeed * Time.deltaTime;

			Vector3 t_dir = (m_tfTarget.position - transform.position).normalized;
			transform.up = Vector3.Lerp(transform.up, t_dir, 0.25f);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		ContactPoint contact = collision.contacts[0];
		Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
		GameObject explosion = Pool.Instance.Spawn("missile_Hit", contact.point, rot);
		Pool.Instance.Despawn(explosion, 3f);
		Pool.Instance.Despawn(this.gameObject);
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.GetComponent<Playerctrl>().TakePlayerDamage(damage);
			if (collision.gameObject.GetComponent<Playerctrl>().currentplayerHP <= 0)
			{
				collision.gameObject.GetComponent<Playerctrl>().OnDie();
			}
			else
			{
				collision.gameObject.GetComponent<Playerctrl>().TakePlayerDamage_animator();
			}
		}
	}


}
