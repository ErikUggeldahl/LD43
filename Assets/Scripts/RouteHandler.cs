using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RouteHandler
{
    bool HasAvailableRoutes();

    bool CanRouteTo(GameObject to);

    void AddRouteTo(GameObject to);

    void AddRouteFrom(GameObject from);

    void Receieve(GameObject traveller);
}
