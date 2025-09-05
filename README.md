# Music EdTech Startup Test

## Giới thiệu

Đây là dự án bài test tập trung vào xây dựng gameplay Rhythm Tap trong Unity. Dự án sử dụng C# và một số thư viện hỗ trợ xử lý file MIDI. Phần này sẽ nêu bật các điểm đã hoàn thành và các phần còn thiếu sót.

---

## Tổng quan

## Tổng quan

- Khi khởi động ứng dụng, người chơi được đưa trực tiếp vào màn chơi Rhythm Tap mà không qua các màn hình lựa chọn hay menu phụ.
- Khi bài nhạc kết thúc, game sẽ tự động phát lại từ đầu.
- Trong quá trình chơi, các note nhạc sẽ di chuyển từ phải sang trái trên màn hình. Khi note đi vào vùng "đánh" nằm bên cạnh khóa Sol, người chơi cần nhấn đúng phím đàn tương ứng (từ C đến B) theo thứ tự các note xuất hiện. Mỗi lần nhấn đúng sẽ ảnh hưởng trực tiếp đến điểm số, chuỗi combo và tốc độ BPM của bài nhạc.
  
---

## Điểm đã làm được

1. **Gameplay Rhythm Tap**  
   - Đã xây dựng gameplay cơ bản: tính điểm khi nhấn đúng note, xử lý nhịp trúng/miss liên tiếp.  
   - Hệ thống thay đổi BPM dựa trên số lần miss và combo trúng liên tiếp, tạo độ thử thách và phản hồi cho người chơi.

2. **Xử lý file MIDI**  
   - Sử dụng thư viện để đọc file MIDI, sinh ra các note tương ứng trong game.  
   - Tốc độ note được điều chỉnh theo BPM động, giúp gameplay sát với nhạc gốc và có thể thích ứng.

---

## Điểm chưa làm được

1. **Kiến trúc dự án**  
   - Dự án hiện tại có kiến trúc còn đơn giản, chủ yếu tập trung vào gameplay cho 1 bài hát duy nhất.  
   - Chưa có các layer hoặc module tách biệt (ví dụ: quản lý nhiều bài hát, quản lý flow game,...).
   - Chưa khai thác đúng cách các design pattern, hiện chỉ sử dụng các pattern đơn giản như singleton và observer.

2. **Log & Remote Config**  
   - Chưa triển khai hệ thống log thông tin hoặc các biến để điều chỉnh từ xa (remote config), gây hạn chế khi cần theo dõi hiệu suất và cấu hình nhanh.

---

