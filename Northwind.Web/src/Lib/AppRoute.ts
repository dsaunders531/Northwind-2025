// AppRoute.ts
// The object used on each line in AppRoutes.tsx

export type AppRoute =
{
        name: string,
        path: string,
        index: boolean,
        element: JSX.Element,
        requireAuth: boolean,
        iconClass: string,
        sortOrder: number
}