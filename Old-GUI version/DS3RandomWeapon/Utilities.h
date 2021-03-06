#pragma once

#include <Windows.h>
#include <vector>
#include <regex>
#include <iostream>
#include <fstream>
#include <TlHelp32.h>
#include "Weapons_list.h"

namespace Utils {
	std::vector<std::string> split(const std::string& str, const std::string& delim)
	{
		std::vector<std::string> tokens;
		size_t prev = 0, pos = 0;
		do
		{
			pos = str.find(delim, prev);
			if (pos == std::string::npos) pos = str.length();
			std::string token = str.substr(prev, pos - prev);
			if (!token.empty()) tokens.push_back(token);
			prev = pos + delim.length();
		} while (pos < str.length() && prev < str.length());
		return tokens;
	}

	bool isNumber(std::string x) {
		std::regex e("^-?\\d+");
		if (std::regex_match(x, e)) return true;
		else return false;
	}

	int Weaponsfcs(std::vector<std::string> weaponlist)
	{
		srand(clock());

		int count = static_cast<int>(weaponlist.size());//old style: sizeof(weaponlist) / sizeof(weaponlist[0]);
		int RandomWeaponIndex = rand() % count; //random number 
		bool isbossweapon = false;

		std::string cur = split(weaponlist[RandomWeaponIndex], " ")[0];

		for (int i = 0; i < boss_weaponlist.size(); i++) { // Check all boss weapons, we should really just do 2 seperat lists, but meh, this is easier
			std::string curwpn = split(boss_weaponlist[i], " ")[0];
			if (cur == curwpn) { isbossweapon = true; }
		}

		// Check for errors, used when debugging
#ifdef _DEBUG
		for (int i = 0; i < cur.length(); ++i) {
			const char* current = &cur[i];
			if (!isNumber(current)) {
				std::cout << "Invalid character in (" << cur << ")(" << cur[i] << ")" << std::endl;
				cur.erase(i, 1);
			}
		}
#endif

		long int Weapon;
		if (!cur.empty()) {
			Weapon = stoi(cur);

			int UpgradeLvl;
			if (isbossweapon) {
				UpgradeLvl = rand() % 6;
			} else {
				UpgradeLvl = rand() % 11;
			}
			Weapon = Weapon + UpgradeLvl;
		}
		else {
			std::cout << "Invalid position in string (" << RandomWeaponIndex << ")" << std::endl;
			Weapon = 2030000;
		}
		return Weapon;
	}

	__int64 GetModuleBaseAddress(LPCWSTR szProcessName, LPCWSTR szModuleName)
	{
		HANDLE hSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
		PROCESSENTRY32 pe32;

		if (hSnap == INVALID_HANDLE_VALUE)
		{
			return 0;
		}
		pe32.dwSize = sizeof(PROCESSENTRY32);
		if (Process32First(hSnap, &pe32) == 0)
		{
			CloseHandle(hSnap);
			return 0;
		}

		do
		{
			if (lstrcmp(pe32.szExeFile, szProcessName) == 0)
			{
				int PID;
				PID = pe32.th32ProcessID;

				HANDLE hSnap = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, PID);
				MODULEENTRY32 xModule;

				if (hSnap == INVALID_HANDLE_VALUE)
				{
					return 0;
				}
				xModule.dwSize = sizeof(MODULEENTRY32);
				if (Module32First(hSnap, &xModule) == 0)
				{
					CloseHandle(hSnap);
					return 0;
				}

				do
				{
					if (lstrcmp(xModule.szModule, szModuleName) == 0)
					{
						CloseHandle(hSnap);
						return (__int64)xModule.modBaseAddr;
					}
				} while (Module32Next(hSnap, &xModule));
				CloseHandle(hSnap);
				return 0;
			}
		} while (Process32Next(hSnap, &pe32));
		CloseHandle(hSnap);
		return 0;
	}

	DWORD FindProcessId(const std::wstring& processName)
	{
		PROCESSENTRY32 processInfo;
		processInfo.dwSize = sizeof(processInfo);

		HANDLE processesSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, NULL);
		if (processesSnapshot == INVALID_HANDLE_VALUE)
			return 0;

		Process32First(processesSnapshot, &processInfo);
		if (!processName.compare(processInfo.szExeFile))
		{
			CloseHandle(processesSnapshot);
			return processInfo.th32ProcessID;
		}

		while (Process32Next(processesSnapshot, &processInfo))
		{
			if (!processName.compare(processInfo.szExeFile))
			{
				CloseHandle(processesSnapshot);
				return processInfo.th32ProcessID;
			}
		}

		CloseHandle(processesSnapshot);
		return 0;
	}
}
