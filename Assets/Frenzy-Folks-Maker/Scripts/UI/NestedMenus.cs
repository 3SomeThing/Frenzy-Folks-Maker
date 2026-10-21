using System;
using System.Collections.Generic;
using UnityEngine;

public class NestedMenus : MonoBehaviour
{
    [SerializeField] protected GameObject mainMenu;

    private readonly List<GameObject> _menus = new List<GameObject>();

    protected void Start()
    {
        _menus.Add(mainMenu);
    }

    public void OpenMenu(GameObject menu)
    {
        if (menu == null) return;
        if (_menus.Count > 0 && _menus[^1] == menu) return;
        _menus[^1].SetActive(false);
        menu.SetActive(true);
        _menus.Add(menu);
    }
    
    public void OnBackClick()
    {
        if (_menus.Count <= 1) return;
        _menus[^1].SetActive(false);
        _menus.RemoveAt(_menus.Count - 1);
        _menus[^1].SetActive(true);
    }
    
    public void CloseAllMenus()
    {
        for (int i = _menus.Count - 1; i >= 0; i--)
        {
            _menus[i].SetActive(false);
            _menus.RemoveAt(i);
        }
    }
    
    public void CloseAllSubMenus()
    {
        for (int i = _menus.Count - 1; i > 0; i--)
        {
            _menus[i].SetActive(false);
            _menus.RemoveAt(i);
        }
        _menus[0].SetActive(true);
    }
}
