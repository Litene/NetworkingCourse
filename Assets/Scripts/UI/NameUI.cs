using System;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class NameUI : MonoBehaviour {

	[SerializeField] private PlayerData _data;
	[SerializeField] private TextMeshProUGUI _text;
	private void Start() => _data.AccountName.OnValueChanged += UpdateText;

	private void UpdateText(FixedString64Bytes playerNamePrev, FixedString64Bytes playerNameCurr) {
		Debug.Log("Race Condition?");
		_text.text = playerNameCurr.Value;
	}

}