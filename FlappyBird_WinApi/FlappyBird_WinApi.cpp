#include "stdafx.h"
#include "commdlg.h"
#include "FlappyBird_WinApi.h"


#define MAX_LOADSTRING 100
#define ID_TIMER 1001
#define ID_NPCTIMER 1002
#define win_width 600
#define win_height 400
#define ball_diameter 16
#define ID_CLOSE 2000
#define ID_EXIT 2001
#define IDM_COLOR 2002


// Global Variables:
HINSTANCE hInst;								// current instance
TCHAR szTitle[MAX_LOADSTRING];					// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name
HWND hWnd;
HWND ball;
HWND obstacle1, obstacle2, obstacle3;
// Forward declarations of functions included in this code module:
ATOM				MyRegisterClass(HINSTANCE hInstance);
ATOM				MyRegisterClassball(HINSTANCE hInstance);
ATOM				MyRegisterClassObstacle(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
LRESULT CALLBACK	ballProc(HWND, UINT, WPARAM, LPARAM);

int APIENTRY _tWinMain(_In_ HINSTANCE hInstance,
	_In_opt_ HINSTANCE hPrevInstance,
	_In_ LPTSTR    lpCmdLine,
	_In_ int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

	// TODO: Place code here.
	MSG msg;
	HACCEL hAccelTable;

	// Initialize global strings
	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadString(hInstance, IDC_FLAPPYBIRDWINAPI, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);
	MyRegisterClassball(hInstance);
	MyRegisterClassObstacle(hInstance);
	// Perform application initialization:
	if (!InitInstance(hInstance, nCmdShow))
	{
		return FALSE;
	}

	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_FLAPPYBIRDWINAPI));

	// Main message loop:
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int)msg.wParam;
}

//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc = WndProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_FLAPPYBIRDWINAPI));
	wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground = (HBRUSH)(CreateSolidBrush(RGB(0, 255, 255)));
	wcex.lpszMenuName = MAKEINTRESOURCE(IDC_FLAPPYBIRDWINAPI);
	wcex.lpszClassName = szWindowClass;
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}

ATOM MyRegisterClassball(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc = ballProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_FLAPPYBIRDWINAPI));
	wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground = (HBRUSH)(CreateSolidBrush(RGB(255, 0, 0)));
	wcex.lpszMenuName = MAKEINTRESOURCE(IDC_FLAPPYBIRDWINAPI);
	wcex.lpszClassName = L"ballclass";
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}

ATOM MyRegisterClassObstacle(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc = ballProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_FLAPPYBIRDWINAPI));
	wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground = (HBRUSH)(CreateSolidBrush(RGB(0, 0, 255)));
	wcex.lpszMenuName = MAKEINTRESOURCE(IDC_FLAPPYBIRDWINAPI);
	wcex.lpszClassName = L"obstacleClass";
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
	hInst = hInstance; // Store instance handle in our global variable

	hWnd = CreateWindow(szWindowClass, L"Flappy Bird", WS_OVERLAPPED | WS_MINIMIZEBOX | WS_SYSMENU,
		500, 500, win_width, win_height, NULL, NULL, hInstance, NULL);
	if (!hWnd)
	{
		return FALSE;
	}
	// Set WS_EX_LAYERED on this window 
	SetWindowLong(hWnd, GWL_EXSTYLE,
		GetWindowLong(hWnd, GWL_EXSTYLE) | WS_EX_LAYERED);
	SetLayeredWindowAttributes(hWnd, 0, (255 * 80) / 100, LWA_ALPHA);

	// Show the window
	ShowWindow(hWnd, nCmdShow);
	UpdateWindow(hWnd);

	//create a ball instance
	ball = CreateWindow(L"ballclass", szTitle, WS_VISIBLE | WS_CHILD,
		150, 150, ball_diameter, ball_diameter, hWnd, NULL, GetModuleHandle(NULL), NULL);
	//make window round
	SetWindowRgn(ball, (HRGN)CreateRoundRectRgn(0, 0, ball_diameter, ball_diameter, ball_diameter, ball_diameter), TRUE);
	ShowWindow(ball, nCmdShow);
	UpdateWindow(ball);

	obstacle1 = CreateWindow(L"obstacleClass", szTitle, WS_VISIBLE | WS_CHILD,
		300, 200, ball_diameter, 500, hWnd, NULL, GetModuleHandle(NULL), NULL);
	//make window round
	ShowWindow(obstacle1, nCmdShow);
	UpdateWindow(obstacle1);

	obstacle2 = CreateWindow(L"obstacleClass", szTitle, WS_VISIBLE | WS_CHILD,
		400, 200, ball_diameter, 600, hWnd, NULL, GetModuleHandle(NULL), NULL);
	//make window round
	ShowWindow(obstacle2, nCmdShow);
	UpdateWindow(obstacle2);

	obstacle3 = CreateWindow(L"obstacleClass", szTitle, WS_VISIBLE | WS_CHILD,
		500, 100, ball_diameter, 500, hWnd, NULL, GetModuleHandle(NULL), NULL);
	//make window round
	ShowWindow(obstacle3, nCmdShow);
	UpdateWindow(obstacle3);

	return TRUE;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;
	HBRUSH brush;


	switch (message)
	{


	case WM_NCHITTEST:
		return HTCAPTION;
	case WM_COMMAND:
		wmId = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{

			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;

		break;

	case WM_DESTROY:
		KillTimer(hWnd, ID_TIMER);
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

LRESULT CALLBACK ballProc(HWND ball, UINT message, WPARAM wParam, LPARAM lParam) {
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;
	HBRUSH brush;
	//set initial ball position and direction
	static int x = win_width / 2;
	static int y = win_height / 2;
	static int dx = 1;
	static int dy = -1;

	switch (message)
	{
	case WM_CREATE:
		SetTimer(ball, ID_TIMER, 16, NULL);
		break;

	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(ball, message, wParam, lParam);
	}
	return 0;
}