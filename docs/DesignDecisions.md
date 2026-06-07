# Route Entity

## Why private setters?

To prevent arbitrary modifications from outside the entity.

## Why constructor validation?

A Route must always have a valid RouteCode and Name.

## Why not use public setters?

Public setters allow invalid states such as empty route names.
