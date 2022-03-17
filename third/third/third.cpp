//#include <iostream>
//#include <thread>
//#include <chrono>
//
//using namespace std;
//
//int trigger;
//
//void naseokhun() {
//	while (trigger == 0)
//	{
//		
//		cout << "정석\n";
//		
//		this_thread::sleep_for(std::chrono::milliseconds(1000));
//	}
//	
//}
//
//void ohjeongseok() {
//	while (trigger == 0)
//	{
//		this_thread::sleep_for(std::chrono::milliseconds(500));
//
//		cout << "석훈\n";
//
//		this_thread::sleep_for(std::chrono::milliseconds(500));
//	}
//	
//}
//
//int main() 
//{
//	trigger = 0;
//	cout << "어렵넿\n";
//	thread na(naseokhun);
//	thread oh(ohjeongseok);
//	while (trigger == 0)
//	{
//		cin >> trigger;
//	}
//	na.join();
//	oh.join();
//	cout << "진짜어렵네\n";
//
//
//}