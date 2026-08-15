## Thông tin dự án

- Unity: `6000.4.2f1` (Unity 6.4 LTS)
- Scene chính: `Assets/Scenes/GamePlay.unity`
- Nền tảng chạy: Windows Editor

## Cách chạy

1. Clone repository và mở thư mục project bằng Unity Hub với đúng phiên bản Unity nêu trên.
2. Mở scene `Assets/Scenes/GamePlay.unity`.
3. Nhấn Play trong Unity Editor.

## Video demo

Trong video demo, sát thương đã được tăng để việc kiểm thử các tính năng combat diễn ra nhanh và dễ quan sát hơn.

## Điều khiển

Di chuyển: WASD, phím mũi tên, tay cầm trái hoặc joystick ảo |
Đánh thường: Nút `Basic Attack`
Đặt bom: Nút `Skill 1` |
Dash rồi nổ: Nút `Skill 2` |

Hướng bắn và dash sử dụng hướng forward hiện tại của nhân vật. Vì vậy khi đổi hướng 180°, nhân vật cần khoảng một giây để xoay xong trước khi hướng skill đổi hoàn toàn.

## Các phần đã thực hiện

### Player và combat

- Player khởi đầu với 500 HP, tốc độ 2 unit/giây, tốc độ xoay 180°/giây, 0 giáp và Damage Multiplier 0.
- Áp dụng công thức sát thương nhận `max(0, base damage - armor)` và sát thương gây ra `base damage * (1 + damage multiplier)`.
- Đánh thường bắn đồng thời 3 viên đạn theo góc -15°, 0°, +15°; mỗi viên có sát thương gốc 10.
- Đánh thường có 3 charge, mỗi lần bắn dùng 1 charge, hồi 1 charge mỗi 3 giây và có khoảng cách tối thiểu 0.5 giây giữa hai lần bắn.
- Bom đặt tại vị trí player, nổ sau 2 giây trong bán kính 5 unit, gây 50 sát thương gốc; cooldown 12 giây.
- Dash theo forward 3 unit trong 0.5 giây, sau đó nổ trong bán kính 3 unit với 15 sát thương gốc; cooldown 6 giây.

### Enemy, wave và progression

- Quái melee: 220 HP, tốc độ 3; tấn công hình nón 50° trong tầm 1.3 unit, gây 30 sát thương gốc, sau đó nghỉ 1 giây.
- Quái ranged: 180 HP, tốc độ 2.7; giữ khoảng cách 3 unit và bắn đạn độc tốc độ 10, tối đa 5 unit.
- Độc gây 30 sát thương gốc mỗi tick: tick ngay khi trúng và thêm mỗi giây trong 3 giây (tổng 4 tick). Dính lại chỉ refresh thời gian, không cộng dồn.
- Mỗi wave spawn ngẫu nhiên 3–4 melee và 1–2 ranged. Wave tiếp theo chỉ bắt đầu khi wave hiện tại đã bị clear.
- Mỗi quái cho 30 EXP. Đủ 100 EXP sẽ lên cấp và giữ EXP dư; khi lên cấp player nhận +40 HP hiện tại, +40 HP tối đa, +2 giáp và +0.1 Damage Multiplier.

### UI và feedback

- Thanh HP và level của player.
- Joystick ảo cùng các nút dùng kỹ năng, có hiển thị cooldown.
- Thanh HP world-space trên đầu từng enemy.
- Có audio và camera shake cho các sự kiện combat chính.

## Tổ chức và khả năng mở rộng

- Chỉ số và skill được tách thành component Inspector và ScriptableObject trong `Assets/_Data/Characters/**/Skill`, thuận tiện để tuning mà không cần sửa luồng gameplay.
- Enemy, projectile, bomb và VFX sử dụng `PoolManager` để tái sử dụng object.
- `EnemyWaveSpawner` dùng danh sách `Wave Entries` trong Inspector; có thể thêm loại enemy hoặc điều chỉnh số lượng spawn mà không đổi code.
- Skill definition/effect tách riêng để mở rộng skill mới mà không sửa controller chung.
