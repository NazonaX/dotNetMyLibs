#include "stdafx.h"
#include "PointerAndReferenceTest.h"

using namespace std;

void PARTST::condition1(int a) {
	a++;
	cout << "in condition1: " << a << endl << "Mem:" << &a << endl;
}

void PARTST::condition2(int &a) {
	a++;
	cout << "in condition2: " << a << endl << "Mem:" << &a << endl;
}

void PARTST::condition3(int *a) {
	(*a)++;
	cout << "in condition3: " << *a << endl << "Mem:" << a << endl;
	int b = -1;
	a = &b;
}